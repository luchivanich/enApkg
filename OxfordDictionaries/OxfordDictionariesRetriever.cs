using Cards;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OxfordDictionaries.DataModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace OxfordDictionaries
{
    public class OxfordDictionariesRetriever : IDictionaryDataRetriever
    {
        private OxfordDictionarySettings _oxfordDictionarySettings;
        private IOxfordDictionariesCacheDBContext _oxfordDictionariesCacheDBContext;
        private IFileDownloader _fileDownloader;

        public OxfordDictionariesRetriever(IOxfordDictionarySettingsProvider oxfordDictionarySettingsProvider, IOxfordDictionariesCacheDBContext oxfordDictionariesCacheDBContext, IFileDownloader fileDownloader)
        {
            _oxfordDictionarySettings = oxfordDictionarySettingsProvider.GetOxfordDictionarySettings();
            _oxfordDictionariesCacheDBContext = oxfordDictionariesCacheDBContext;
            _fileDownloader = fileDownloader;
        }
        
        public (string definition, List<string> examples, string fileName, byte[] fileData) GetDictionaryData(IWord word)
        {
            var lexicalEntry = RetrieveOdLexicalEntry(word);
            var sense = lexicalEntry
                ?.OxfordDictionaryLexicalEntryV2
                ?.Entries
                ?.FirstOrDefault()?
                .Senses?
                .FirstOrDefault();
            return (
                sense?.Definitions?.FirstOrDefault(),
                sense?.Examples?.Select(e => e.Text).ToList(),
                lexicalEntry?.AudioFile?.FileName,
                lexicalEntry?.AudioFile?.Data);
        }

        private LexicalEntry RetrieveOdLexicalEntry(IWord word)
        {
            return _oxfordDictionariesCacheDBContext
                .LexicalEntries
                .Include(le => le.AudioFile)
                .FirstOrDefault(l => l.Word.ToLower() == word.Word.ToLower() && (l.LexicalCategory == word.LexicalCategory.ToString() || word.LexicalCategory == null))
                ??
                RetrieveLexicalEntriesFromOdApi(word.Word)
                .FirstOrDefault(l => l.Word.ToLower() == word.Word.ToLower() && l.LexicalCategory == word.LexicalCategory.ToString() || word.LexicalCategory == null);
        }

        private List<LexicalEntry> RetrieveLexicalEntriesFromOdApi(string word)
        {
            var client = new HttpClient();
            var query = BuildApiQuery(word);

            client.DefaultRequestHeaders.Add("APP_ID", _oxfordDictionarySettings.AppId);
            client.DefaultRequestHeaders.Add("APP_KEY", _oxfordDictionarySettings.ApiKey);
            var response = client.GetAsync(query);

            Thread.Sleep(1000); // TODO

            var a = response.Result;
            switch (a.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var resultString = a.Content.ReadAsStringAsync().Result;
                    return CreateLexicalEntryFromOxfordDictionaryResponse(word, resultString);

                case System.Net.HttpStatusCode.NotFound:
                    return CreateLexicalEntryFromOxfordDictionaryResponse(word, string.Empty);
            }
            return new List<LexicalEntry>();
        }

        private string BuildApiQuery(string word)
        {
            return $"{_oxfordDictionarySettings.BaseUrl}/entries/en/{word}";
        }

        private List<LexicalEntry> CreateLexicalEntryFromOxfordDictionaryResponse(string word, string response)
        {
            var odData = JsonConvert.DeserializeObject<OxfordDictionaryEntityV2>(response);
            var result = new List<LexicalEntry>();
            var audioFiles = new List<AudioFile>();

            var odLexicalEntries = odData?.Results?.FirstOrDefault()?.LexicalEntries;

            if (odLexicalEntries?.Count > 0)
            {
                foreach (var le in odLexicalEntries)
                {
                    var lexicalEntry = new LexicalEntry
                    {
                        Word = le.Text,
                        LexicalCategory = le.LexicalCategory.Text,
                        OxfordDictionaryLexicalEntryV2Json = JsonConvert.SerializeObject(le)
                    };

                    var audioFileUri = le.Pronunciations?.FirstOrDefault()?.AudioFile;
                    if (audioFileUri != null)
                    {
                        var audioFile = audioFiles.FirstOrDefault(af => af.Url == audioFileUri.AbsoluteUri) 
                            ?? _oxfordDictionariesCacheDBContext.AudioFiles.FirstOrDefault(af => af.Url == audioFileUri.AbsoluteUri);
                        if (audioFile == null)
                        {
                            audioFile = new AudioFile
                            {
                                Url = audioFileUri.AbsoluteUri,
                                FileName = Path.GetFileName(audioFileUri.LocalPath),
                                Data = _fileDownloader.GetFileFromUrl(audioFileUri.AbsoluteUri),
                                LexicalEntries = new List<LexicalEntry>()
                            };
                            audioFiles.Add(audioFile);
                        }
                        lexicalEntry.AudioFile = audioFile;
                    }
                    _oxfordDictionariesCacheDBContext.LexicalEntries.Add(lexicalEntry);
                    result.Add(lexicalEntry);
                }
            }
            else
            {
                var lexicalEntry = new LexicalEntry { Word = word };
                result.Add(lexicalEntry);
                _oxfordDictionariesCacheDBContext.LexicalEntries.Add(lexicalEntry);
            }
            _oxfordDictionariesCacheDBContext.SaveChanges();
            return result;
        }
    }
}
