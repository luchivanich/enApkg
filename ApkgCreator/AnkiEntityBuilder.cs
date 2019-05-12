using System;
using System.IO;
using ApkgCreator.DataModels;
using Cards;

namespace ApkgCreator
{
    public class AnkiEntityBuilder : IAnkiEntityBuilder
    {
        private IAnkiFieldsBuilder _ankiFieldsBuilder;
        private IAnkiAdditionalModelsBuilder _ankiAdditionalModelsBuilder;

        public AnkiEntityBuilder(IAnkiFieldsBuilder ankiFieldsBuilder, IAnkiAdditionalModelsBuilder ankiAdditionalModelsBuilder)
        {
            _ankiFieldsBuilder = ankiFieldsBuilder;
            _ankiAdditionalModelsBuilder = ankiAdditionalModelsBuilder;
        }

        public AnkiCol BuildAnkiCol(long deckId, long modelId)
        {
            var conf = _ankiAdditionalModelsBuilder.BuildAnkiCol();
            var dconf = _ankiAdditionalModelsBuilder.BuildAnkiDeckConfig(1);
            var decks = _ankiAdditionalModelsBuilder.BuildAnkiDeckInfo(deckId, "MAIN DECK!!!");
            var models = File.ReadAllText(@"col.models.txt");
            models = models.Replace("@modelId", modelId.ToString());
            var tags = "{}";

            return new AnkiCol()
            {
                Id = 1,
                Created = 1413766800,
                Modified = 1413830407602,
                SchemaModeTime = 1413830406248,
                Ver = 11,
                Dty = 0,
                Usn = 0,
                LastSyncTime = 0,
                Conf = conf,
                Models = models,
                Decks = decks,
                DeckConfiguration = dconf,
                Tags = tags,
            };  
        }

        public AnkiCard BuildAnkiCard(Card card, long deckId, long modelId)
        {
            var fields = _ankiFieldsBuilder.BuildFields(card);

            var ankiNote = new AnkiNote
            {
                Guid = Guid.NewGuid().ToString(),
                ModleId = modelId,
                Modified = 1413830378,
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
                DeckId = deckId,
                Ordinal = 0,
                ModifiedTime = 1413830378,
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
    }
}
