using ApkgCreator;
using Cards;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace consoleApp
{
    [Serializable]
    public class CardTmp
    {
        public string Word { get; set; }
        public string InitialLexicalCategory { get; set; }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            CreateApkg(@"E:\programming\longman9000\", @"testPackage");
            return;

            //var values = new Dictionary<string, string>
            //{
            //    {"key", "trnsl.1.1.20190313T202055Z.030f36cee5cde73c.166a4a6f971e486b32971051ca453ee73ca953e5"},
            //    {"text", "girl"},
            //    {"lang", "en-ru"}
            //};

            //var content = new FormUrlEncodedContent(values);
            ////var response = client.PostAsync("https://translate.yandex.net/api/v1.5/tr.json/translate", content).Result;
            //var response = client.GetAsync(@"https://www.dictionary.com/browse/mother").Result;
            //var responseString = response.Content.ReadAsStringAsync().Result;

            //Console.WriteLine(responseString);

            // Generate Longman 9000
            //var high = JsonConvert.DeserializeObject<List<CardTmp>>(File.ReadAllText(@"E:\programming\longman9000\easy.json"));
            //var newEasy = TransforCardList(high, Frequency.High);

            //var medium = JsonConvert.DeserializeObject<List<CardTmp>>(File.ReadAllText(@"E:\programming\longman9000\medium.json"));
            //var newMedium = TransforCardList(medium, Frequency.Medium);

            //var low = JsonConvert.DeserializeObject<List<CardTmp>>(File.ReadAllText(@"E:\programming\longman9000\hard.json"));
            //var newLow = TransforCardList(low, Frequency.Low);

            //var result = newEasy;
            //result.AddRange(newMedium);
            //result.AddRange(newLow);

            //result = result.GroupBy(i => new { i.Word, i.LexicalCategory, i.Frequency }).Select(g => new Card { Word = g.Key.Word, LexicalCategory = g.Key.LexicalCategory, Frequency = g.Key.Frequency }).ToList();

            //File.WriteAllText(@"E:\programming\longman9000\longman9000.json", JsonConvert.SerializeObject(result));
            // Generate Longman 9000 end

            //var wordRetriever = new DictionaryComRetriever();
            //foreach (var item in result)
            //{
            //    var definition = wordRetriever.RetrieveDefinition(item.WordItself);
            //    item.Definition = definition;
            //}

            //File.WriteAllText(@"E:\programming\longman9000\easyWithDefinitions.json", JsonConvert.SerializeObject(result));

            //var wordRetriever = new OxfordDictionariesRetriever.OxfordDictionariesRetriever();
            //wordRetriever.ResetDataBase();
            //var a = wordRetriever.RetrieveDefinition("about");

            //Console.WriteLine(a);

            //Console.Read();

            //var oxfordRetriever = new OxfordDictionariesRetriever.OxfordDictionariesRetriever();
            //var cardBuilder = new CardBuilder(oxfordRetriever, oxfordRetriever);
            //var card = cardBuilder.BuildCard(new LongmanWord { Word = "test", LexicalCategory = LexicalCategory.Noun });
            //Console.WriteLine(card.Definition);
            //foreach (var e in card.Examples)
            //{
            //    Console.WriteLine("\t" + e);
            //}
            //card = cardBuilder.BuildCard(new LongmanWord { Word = "test", LexicalCategory = LexicalCategory.Verb });
            //Console.WriteLine(card.Definition);
            //foreach (var e in card.Examples)
            //{
            //    Console.WriteLine("\t" + e);
            //}


            //var words = JsonConvert.DeserializeObject<List<LongmanWord>>(File.ReadAllText(@"E:\programming\longman9000\longman9000.json"));
            //var cardDbService = new CardDbService();
            //using (var db = new CardsDbContext())
            //{
            //    foreach (var i in words)
            //    {
            //        var card = new Card { Word = i.Word, LexicalCategory = i.LexicalCategory, Frequency = i.Frequency };
            //        cardDbService.SaveCard(db, card);
            //    }
            //    var count = db.SaveChanges();
            //    Console.WriteLine($"Done: {count}");
            //}

            var oxfordRetriever = new OxfordDictionariesRetriever.OxfordDictionariesRetriever();
            var cardBuilder = new CardBuilder(oxfordRetriever, oxfordRetriever);
            var cardDbService = new CardDbService();

            var random = new Random();
            using (var db = new CardsDbContext())
            {
                for (var i = 0; i < 100; i++)
                {
                    var cards = db.Cards.ToList();
                    int index = random.Next(cards.Count());
                    var card = cards[index];
                    if (string.IsNullOrEmpty(card.Definition))
                    {
                        var cardId = card.Id;
                        var builtCard = cardBuilder.BuildCard(card);
                        builtCard.Id = cardId;
                        cardDbService.SaveCard(db, builtCard);
                    }
                    System.Threading.Thread.Sleep(1001);
                }

                var count = db.SaveChanges();
                Console.WriteLine($"Done: {count}");
            }


            Console.Read();
        }

        private static void CreateApkg(string directory, string targetName)
        {
            var apkgBuilder = new ApkgBuilder();
            apkgBuilder.Init(Path.Combine(directory, targetName));

            //var card = CreateCard("test");
            foreach (var card in GetCards())
            {
                apkgBuilder.AddCard(card);
            }

            apkgBuilder.CreateApkgFile(directory, targetName);
            Console.WriteLine("Apkg directory was created");
        }

        private static Card CreateCard(string word)
        {
            var oxfordRetriever = new OxfordDictionariesRetriever.OxfordDictionariesRetriever();
            var cardBuilder = new CardBuilder(oxfordRetriever, oxfordRetriever);
            var cardDbService = new CardDbService();

            using (var db = new CardsDbContext())
            {
                var card = db.Cards.ToList().FirstOrDefault(c => c.Word == word);
                if (string.IsNullOrEmpty(card.Definition))
                {
                    var cardId = card.Id;
                    var builtCard = cardBuilder.BuildCard(card);
                    builtCard.Id = cardId;
                    cardDbService.SaveCard(db, builtCard);
                }

                var count = db.SaveChanges();
                Console.WriteLine($"Done: {count}");

                return card;
            }
        }

        private static List<Card> GetCards()
        {
            using (var db = new CardsDbContext())
            {
                return db.Cards.Where(c => !string.IsNullOrEmpty(c.Definition)).ToList();
            }
        }

        private static List<Card> TransforCardList(List<CardTmp> cards, Frequency frequency)
        {
            var result = new List<Card>();

            foreach (var i in cards)
            {
                var lexicalCategories = i.InitialLexicalCategory?.Split(',');

                if (lexicalCategories != null)
                {
                    foreach (var lc in lexicalCategories)
                    {
                        result.Add(GetCard(i.Word, lc, frequency));
                    }
                }
                else
                {
                    result.Add(GetCard(i.Word, null, frequency));
                }
            }

            return result;
        }

        private static Card GetCard(string word, string lexicalCategory, Frequency frequency)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return new Card
            {
                Word = word,
                LexicalCategory = lexicalCategory == null ? null : (LexicalCategory?)Enum.Parse(typeof(LexicalCategory), cultureInfo.TextInfo.ToTitleCase(lexicalCategory).Replace(" ", "")),
                Frequency = frequency
            };
        }
    }
}
