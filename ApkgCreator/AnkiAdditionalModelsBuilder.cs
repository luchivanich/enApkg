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

        public string BuildAnkiDeckConfig(long configId)
        {
            var ankiDeckConfig = new AnkiDeckConfig
            {
                DeckConfig = new DeckConfig
                {
                    Name = "Default",
                    Replayq = true,
                    Lapse = new Lapse
                    {
                        LeechFails = 8,
                        MinInt = 1,
                        Delays = new List<long> { 10 },
                        LeechAction = 0,
                        Mult = 0
                    },
                    Rev = new Rev
                    {
                        PerDay = 100,
                        Fuzz = 0.05,
                        IvlFct = 1,
                        MaxIvl = 36500,
                        Ease4 = 1.3,
                        Bury = true,
                        MinSpace = 1
                    },
                    Timer = 0,
                    MaxTaken = 60,
                    Usn = 0,
                    New = new NewItem
                    {
                        PerDay = 20,
                        Delays = new List<long> { 1, 10 },
                        Separate = true,
                        Ints = new List<long> { 1, 4, 7 },
                        InitialFactor = 2500,
                        Bury = true,
                        Order = 1
                    },
                    Mod = 0,
                    Id = configId,
                    Autoplay = true
                }
            };
            return JsonConvert.SerializeObject(ankiDeckConfig, new AnkiDeckConfigJsonConverter());
        }

        public string BuildAnkiDeckInfo(long deckId, string deckName)
        {
            var ankiDeckInfo = new AnkiDeckInfo
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
            return JsonConvert.SerializeObject(ankiDeckInfo, new AnkiDeckInfoJsonConverter());
        }

        public string BuildAnkiModel()
        {
            var ankiModel = new AnkiModel
            {

            };
            return JsonConvert.SerializeObject(ankiModel, new AnkiDeckInfoJsonConverter());
        }
    }
}
