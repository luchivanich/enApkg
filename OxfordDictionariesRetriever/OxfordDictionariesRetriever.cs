using Cards;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace OxfordDictionariesRetriever
{
    public class OxfordDictionariesRetriever : IDefinitionRetriever, IExamplesRetriever
    {
        private const string APP_ID = "284d98d1";
        private const string API_KEY = "a96fc97f75810b2f4d390f00e6b4f0ce";
        private const string BASE_URL = @"https://od-api.oxforddictionaries.com:443/api/v1";

        private const string DEFINITIONS_ENDPOINT = @"/entries/{source_lang}/{word_id}/definitions";

        public OxfordDictionaryEntity RetrieveOxfordDictionaryEntity(string word)
        {
            var result = GetWordDefinitionFromDB(word);

            if (result == null)
            {
                var client = new HttpClient();
                var query = GetWholeWordQuery(word);

                client.DefaultRequestHeaders.Add("APP_ID", APP_ID);
                client.DefaultRequestHeaders.Add("APP_KEY", API_KEY);
                var response = client.GetAsync(query);
                var a = response.Result;
                if (a.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                var resultString = a.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<OxfordDictionaryEntity>(resultString);

                if (result != null)
                {
                    SaveNewWord(word, resultString);
                }

            }

            return result;
        }

        private string GetWholeWordQuery(string word)
        {
            return $"{BASE_URL}/entries/en/{word}";
        }

        private OxfordDictionaryEntity GetWordDefinitionFromDB(string word)
        {
            using (var db = new OxfordDictionariesRetrieverDBContext())
            {
                var cachedItem = db.Words.SingleOrDefault(w => w.Word == word);
                if (cachedItem != null)
                {
                    return JsonConvert.DeserializeObject<OxfordDictionaryEntity>(cachedItem?.OxfordDictionaryEntityJson);
                }
                return null;
            }
        }

        private void SaveNewWord(string word, string entity)
        {
            var itemToSave = new CachedItem { Word = word, OxfordDictionaryEntityJson = entity };

            using (var db = new OxfordDictionariesRetrieverDBContext())
            {
                db.Words.Add(itemToSave);
                var count = db.SaveChanges();
            }
        }

        public void ResetDataBase()
        {
            using (var db = new OxfordDictionariesRetrieverDBContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.SaveChanges();
            }
        }

        public string GetDefinition(LongmanWord word)
        {
            var entity = RetrieveOxfordDictionaryEntity(word.Word);
            return entity?.Results?.FirstOrDefault()?.LexicalEntries?.FirstOrDefault(i => i.LexicalCategory == word.LexicalCategory.ToString())?.Entries?.FirstOrDefault()?.Senses?.FirstOrDefault()?.Definitions?.FirstOrDefault();
        }

        public List<string> GetExamples(LongmanWord word)
        {
            var entity = RetrieveOxfordDictionaryEntity(word.Word);
            return entity?.Results?.FirstOrDefault()?.LexicalEntries?.FirstOrDefault(i => i.LexicalCategory == word.LexicalCategory.ToString())?.Entries?.FirstOrDefault()?.Senses?.FirstOrDefault()?.Examples?.Select(e => e.Text).ToList();
        }
    }
}
