using Cards;
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

        private OxfordDictionaryLexicalEntryV2 RetrieveOdLexicalEntries(IWord word)
        {
            var lexicalEntry = _oxfordDictionariesCacheDBContext
                .LexicalEntries
                .FirstOrDefault(l => l.Word.ToLower() == word.Word.ToLower() && (l.LexicalCategory == word.LexicalCategory.ToString() || word.LexicalCategory == null));

            if (lexicalEntry == null)
            {
                lexicalEntry = RetrieveLexicalEntriesFromOdApi(word.Word)
                    .FirstOrDefault(l => l.Word.ToLower() == word.Word.ToLower() && l.LexicalCategory == word.LexicalCategory.ToString());
            }

            if (lexicalEntry?.OxfordDictionaryLexicalEntryV2Json == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<OxfordDictionaryLexicalEntryV2>(lexicalEntry.OxfordDictionaryLexicalEntryV2Json);
        }

        private List<LexicalEntry> RetrieveLexicalEntriesFromOdApi(string word) // TODO Rework the whole method (approach)
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
                    var odData = JsonConvert.DeserializeObject<OxfordDictionaryEntityV2>(resultString);
                    var result = new List<LexicalEntry>();

                    var odMainResult = odData?.Results?.FirstOrDefault();

                    foreach(var le in odMainResult.LexicalEntries)
                    {
                        var lexicalEntry = new LexicalEntry
                        {
                            Word = odMainResult.Word,
                            LexicalCategory = le.LexicalCategory.Text,
                            OxfordDictionaryLexicalEntryV2Json = JsonConvert.SerializeObject(le)
                        };

                        var audioFileUri = le.Pronunciations?.FirstOrDefault().AudioFile;
                        if (audioFileUri != null)
                        {
                            var audioFile = _oxfordDictionariesCacheDBContext.AudioFiles.FirstOrDefault(af => af.Url == audioFileUri.AbsoluteUri);
                            if (audioFile == null)
                            {
                                audioFile = new AudioFile {
                                    Url = audioFileUri.AbsoluteUri,
                                    FileName = Path.GetFileName(audioFileUri.LocalPath),
                                    Data = _fileDownloader.GetFileFromUrl(audioFileUri.AbsoluteUri),
                                    LexicalEntries = new List<LexicalEntry>() };
                                _oxfordDictionariesCacheDBContext.AudioFiles.Add(audioFile);
                            }
                            audioFile.LexicalEntries.Add(lexicalEntry);
                        }

                        _oxfordDictionariesCacheDBContext.LexicalEntries.Add(lexicalEntry);
                        _oxfordDictionariesCacheDBContext.SaveChanges();
                        result.Add(lexicalEntry);
                    }
                    return result;

                case System.Net.HttpStatusCode.NotFound:
                    _oxfordDictionariesCacheDBContext.LexicalEntries.Add(new LexicalEntry{ Word = word });
                    _oxfordDictionariesCacheDBContext.SaveChanges();
                    return new List<LexicalEntry>();
            }
            return new List<LexicalEntry>();
        }

        private string BuildApiQuery(string word)
        {
            return $"{_oxfordDictionarySettings.BaseUrl}/entries/en/{word}";
        }

        public string GetDefinition(IWord word)
        {
            return GetWordSense(word)?.Definitions?.FirstOrDefault();
        }

        public List<string> GetExamples(IWord word)
        {
            return GetWordSense(word)?.Examples?.Select(e => e.Text).ToList();
        }

        //public Uri GetAudioFileUri(IWord word)
        //{
        //    var entity = RetrieveOxfordDictionaryEntity(word.Word);
        //    return entity?
        //        .Results?
        //        .FirstOrDefault()?
        //        .LexicalEntries?
        //        .FirstOrDefault(i => i.LexicalCategory == word.LexicalCategory.ToString() || word.LexicalCategory == null)?
        //        .Pronunciations?.FirstOrDefault()?.AudioFile;
        //}

        private OxfordDictionarySenseV2 GetWordSense(IWord word)
        {
            var lexicalEntry = RetrieveOdLexicalEntries(word);
            return lexicalEntry?
                .Entries?
                .FirstOrDefault()?
                .Senses?
                .FirstOrDefault();
        }

        public (string fileName, byte[] fileData) GetAudioFile(IWord word)
        {
            throw new System.NotImplementedException();
        }

        //private List<AudioFile> GetAudioFiles(OxfordDictionaryEntity odEntity)
        //{
        //    var urls = odEntity?.Results?.Select(r => r.LexicalEntries?.Sel)
        //}
    }
}
