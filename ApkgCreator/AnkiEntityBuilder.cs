using System;
using System.Collections.Generic;
using System.IO;
using ApkgCreator.AdditionalModels;
using ApkgCreator.DataModels;
using Cards;

namespace ApkgCreator
{
    public class AnkiEntityBuilder : IAnkiEntityBuilder
    {
        private IAnkiFieldsBuilder _ankiFieldsBuilder;

        public AnkiEntityBuilder(IAnkiFieldsBuilder ankiFieldsBuilder)
        {
            _ankiFieldsBuilder = ankiFieldsBuilder;
        }

        public AnkiCol BuildAnkiCol()
        {
            var epoch = new DateTimeOffset().ToUnixTimeSeconds();
            var epochMiliseconds = new DateTimeOffset().ToUnixTimeMilliseconds();

            return new AnkiCol()
            {
                Id = 1,
                Created = epoch,
                Modified = epochMiliseconds,
                SchemaModeTime = epochMiliseconds,
                Ver = 11,
                Dty = 0,
                Usn = 0,
                LastSyncTime = 0,
                Tags = "{}", // TODO
                AnkiModel = BuildAnkiModel(),
                AnkiDeckInfo = BuildAnkiDeckInfo(),
                AnkiColConfig = BuildAnkiColConfig(),
                AnkiDeckConfig = BuildAnkiDeckConfig()
            };  
        }

        public AnkiCard BuildAnkiCard(Card card, AnkiCol ankiCol)
        {
            var fields = _ankiFieldsBuilder.BuildFieldsString(card, ankiCol.AnkiModel.Model.Flds);
            var epoch = new DateTimeOffset().ToUnixTimeSeconds();

            var ankiNote = new AnkiNote
            {
                Guid = Guid.NewGuid().ToString(),
                ModleId = long.Parse(ankiCol.AnkiModel.Model.Id),
                Modified = epoch,
                Usn = -1,
                Tags = string.Empty,
                Fields = fields,
                Sfld = 1,
                Csum = 896145707,
                Flags = 0,
                Data = string.Empty,
            };

            return new AnkiCard
            {
                Note = ankiNote,
                DeckId = long.Parse(ankiCol.AnkiDeckInfo.Deck.Id),
                Ordinal = 0,
                ModifiedTime = epoch,
                Usn = -1,
                Type = 0,
                Queue = 0,
                Due = 1,
                Interval = 0,
                Factor = 2500,
                NumberOfReviews = 0,
                Lapses = 0,
                Left = 1001,
                Odue = 0,
                Odid = 0,
                Flags = 0,
                Data = string.Empty,
            };
        }

        private AnkiColConfig BuildAnkiColConfig()
        {
            return new AnkiColConfig
            {
                NextPos = 1,
                EstTimes = true,
                ActiveDecks = new List<long> { 1 },
                SortType = "noteFld",
                TimeLim = 0,
                SortBackwards = false,
                AddToCur = true,
                CurDeck = 1,
                NewBury = true,
                NewSpread = 0,
                DueCounts = true,
                CurModel = "1413830406249",
                CollapseTime = 1200
            };
        }

        private AnkiDeckConfig BuildAnkiDeckConfig()
        {
            return new AnkiDeckConfig
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
                    Id = 1, // TODO
                    Autoplay = true
                }
            };
        }

        private AnkiDeckInfo BuildAnkiDeckInfo()
        {
            return new AnkiDeckInfo
            {
                Deck = new Deck
                {
                    Desc = string.Empty,
                    Name = $"Deck{DateTime.Now.ToBinary()}", // TODO
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
                    Id = "1413830276206", // TODO: Generate ID
                    Mod = 1413830276
                }
            };
        }

        private AnkiModel BuildAnkiModel()
        {
            return new AnkiModel
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
                            Name = "TestTemplate50", // TODO
                            Qfmt = File.ReadAllText(@"Templates/QuetionTemplate.htm"),
                            Did = null,
                            Bafmt = string.Empty,
                            Afmt = File.ReadAllText(@"Templates/AnswerTemplate.htm"),
                            Ord = 0,
                            Bqfmt = string.Empty
                        }
                    },
                    Mod = 1413830391,
                    LatexPost = "\\end{document}",
                    Type = 1,
                    Id = "1397924728875", // TODO: Generate Id,
                    Css = File.ReadAllText(@"Templates/Styles.css"),
                    LatexPre = "\\documentclass[12pt]{article}\n\\special{papersize=3in,5in}\n\\usepackage[utf8]{inputenc}\n\\usepackage{amssymb,amsmath}\n\\pagestyle{empty}\n\\setlength{\\parindent}{0in}\n\\begin{document}\n"
                }
            };
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
