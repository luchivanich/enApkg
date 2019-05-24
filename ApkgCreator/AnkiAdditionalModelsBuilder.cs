using System.Collections.Generic;
using System.IO;
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

        public string BuildAnkiModel(long modelId)
        {
            var questionTemplate = File.ReadAllText(@"Templates/QuetionTemplate.htm");
            var answerTemplate = File.ReadAllText(@"Templates/AnswerTemplate.htm");
            var styles = File.ReadAllText(@"Templates/Styles.css");

            var ankiModel = new AnkiModel
            {
                Model = new Model
                {
                    Vers = null,
                    Name = "Main",
                    Tags = null,
                    Did = 1399112064926,
                    Usn = -1,
                    Flds = new List<Fld>
                    {
                        CreateFld("№", 0),
                        CreateFld("IMG", 1),
                        CreateFld("English", 2),
                        CreateFld("Keyword", 3),
                        CreateFld("Transcription", 4, "Lucida Sans Unicode"),
                        CreateFld("Russian", 5, "Arial Unicode MS"),
                        CreateFld("Sound", 6),
                        CreateFld("Examples", 7),
                    },
                    Sortf = 0,
                    Tmpls = new List<Tmpl>
                    {
                        new Tmpl
                        {
                            Name = "TestTemplate30",
                            Qfmt = questionTemplate,
                            Did = null,
                            Bafmt = string.Empty,
                            Afmt = answerTemplate,
                            Ord = 0,
                            Bqfmt = string.Empty
                        }
                    },
                    Mod = 1413830391,
                    LatexPost = "\\end{document}",
                    Type = 1,
                    Id = modelId.ToString(),
                    Css = styles,
                    LatexPre = "\\documentclass[12pt]{article}\n\\special{papersize=3in,5in}\n\\usepackage[utf8]{inputenc}\n\\usepackage{amssymb,amsmath}\n\\pagestyle{empty}\n\\setlength{\\parindent}{0in}\n\\begin{document}\n"
                }
            };
            return JsonConvert.SerializeObject(ankiModel, new AnkiModelJsonConverter());
        }

        private Fld CreateFld(string name, int order, string font = "Arial")
        {
            return new Fld
            {
                Name = name,
                Media = null,
                Sticky = false,
                Rtl = false,
                Ord = order,
                Font = font,
                Size = 20
            };
        }
    }
}
