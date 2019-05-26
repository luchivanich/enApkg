using Cards;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace OxfordDictionaries
{
    public class OxfordDictionariesRetriever : IDefinitionRetriever, IExamplesRetriever, IAudioFileUrlRetriever
    {
        private OxfordDictionarySettings _oxfordDictionarySettings;
        private IOxfordDictionariesCacheDBContext _oxfordDictionariesCacheDBContext;

        public OxfordDictionariesRetriever(IOxfordDictionarySettingsProvider oxfordDictionarySettingsProvider, IOxfordDictionariesCacheDBContext oxfordDictionariesCacheDBContext)
        {
            _oxfordDictionarySettings = oxfordDictionarySettingsProvider.GetOxfordDictionarySettings();
            _oxfordDictionariesCacheDBContext = oxfordDictionariesCacheDBContext;
        }

        public OxfordDictionaryEntity RetrieveOxfordDictionaryEntity(string word)
        {
            var cachedItem = _oxfordDictionariesCacheDBContext.Words.SingleOrDefault(w => w.Word == word);

            if (cachedItem != null)
            {
                return JsonConvert.DeserializeObject<OxfordDictionaryEntity>(cachedItem?.OxfordDictionaryEntityJson);
            }

            var client = new HttpClient();
            var query = GetWholeWordQuery(word);

            client.DefaultRequestHeaders.Add("APP_ID", _oxfordDictionarySettings.AppId);
            client.DefaultRequestHeaders.Add("APP_KEY", _oxfordDictionarySettings.ApiKey);
            var response = client.GetAsync(query);

            Thread.Sleep(1000); // TODO

            var a = response.Result;
            switch (a.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var resultString = a.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<OxfordDictionaryEntity>(resultString);
                    SaveNewWord(word, resultString);
                    return result;

                case System.Net.HttpStatusCode.NotFound:
                    SaveNewWord(word, string.Empty);
                    break;
            }
            return null;
        }

        private string GetWholeWordQuery(string word)
        {
            return $"{_oxfordDictionarySettings.BaseUrl}/entries/en/{word}";
        }

        private OxfordDictionaryEntity GetWordDefinitionFromDB(string word)
        {
            var cachedItem = _oxfordDictionariesCacheDBContext.Words.SingleOrDefault(w => w.Word == word);
            if (cachedItem != null)
            {
                return JsonConvert.DeserializeObject<OxfordDictionaryEntity>(cachedItem?.OxfordDictionaryEntityJson);
            }
            return null;
        }

        private void SaveNewWord(string word, string entity)
        {
            var itemToSave = new CachedItem { Word = word, OxfordDictionaryEntityJson = entity };

            _oxfordDictionariesCacheDBContext.Words.Add(itemToSave);
            _oxfordDictionariesCacheDBContext.SaveChanges();
        }

        public void ResetDataBase()
        {
            _oxfordDictionariesCacheDBContext.Database.EnsureDeleted();
            _oxfordDictionariesCacheDBContext.Database.EnsureCreated();

            _oxfordDictionariesCacheDBContext.SaveChanges();
        }

        public string GetDefinition(LongmanWord word)
        {
            return GetWordSense(word)?.Definitions?.FirstOrDefault();
        }

        public List<string> GetExamples(LongmanWord word)
        {
            return GetWordSense(word)?.Examples?.Select(e => e.Text).ToList();
        }

        public Uri GetAudioFileUri(LongmanWord word)
        {
            var entity = RetrieveOxfordDictionaryEntity(word.Word);
            return entity?
                .Results?
                .FirstOrDefault()?
                .LexicalEntries?
                .FirstOrDefault(i => i.LexicalCategory == word.LexicalCategory.ToString() || word.LexicalCategory == null)?
                .Pronunciations?.FirstOrDefault()?.AudioFile;
        }

        private Sense GetWordSense(LongmanWord word)
        {
            var entity = RetrieveOxfordDictionaryEntity(word.Word);
            return entity?
                .Results?
                .FirstOrDefault()?
                .LexicalEntries?
                .FirstOrDefault(i => i.LexicalCategory == word.LexicalCategory.ToString() || word.LexicalCategory == null)?
                .Entries?
                .FirstOrDefault()?
                .Senses?
                .FirstOrDefault();
        }
    }
}
