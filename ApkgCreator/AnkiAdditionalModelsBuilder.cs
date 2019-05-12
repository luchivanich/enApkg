using System.Collections.Generic;
using ApkgCreator.AdditionalModels;
using ApkgCreator.AdditionalModels.Converters;
using Newtonsoft.Json;

namespace ApkgCreator
{
    public class AnkiAdditionalModelsBuilder : IAnkiAdditionalModelsBuilder
    {
        public string BuildAnkiCol()
        {
            var ankiColConfig = new AnkiColConfig
            {
                NextPos = 1,
                EstTimes = true,
                ActiveDecks = new List<long> { 1 },
                SortType = "noteFld",
                TimeLim =  0,
                SortBackwards = false,
                AddToCur = true,
                CurDeck = 1,
                NewBury = true,
                NewSpread = 0,
                DueCounts = true,
                CurModel = "1413830406249",
                CollapseTime = 1200
            };
            return JsonConvert.SerializeObject(ankiColConfig);
        }

        public string BuildAnkiDeckInfo(long deckId, string deckName)
        {
            var ankiDeckConfig = new AnkiDeckInfo
            {
                Deck = new Deck
                {
                    Desc = string.Empty,
                    Name = deckName,
                    ExtendRev = 50,
                    Usn = -1,
                    Collapsed = false,
                    NewToday = new List<long> { 0, 0 },
                    TimeToday = new List<long> { 0, 0 },
                    Dyn = 0,
                    ExtendNew = 10,
                    Conf = 1,
                    RevToday = new List<long> { 0, 0 },
                    LrnToday = new List<long> { 0, 0 },
                    Id = deckId.ToString(),
                    Mod = 1413830276
                }
            };

            return JsonConvert.SerializeObject(ankiDeckConfig, new AnkiDeckInfoJsonConverter());
        }
    }
}
