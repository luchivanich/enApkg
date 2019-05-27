using System.Linq;

namespace Cards
{
    public class CardBuilder : ICardBuilder
    {
        private IDictionaryDataRetriever _dictionaryDataRetriever;

        public CardBuilder(IDictionaryDataRetriever dictionaryDataRetriever)
        {
            _dictionaryDataRetriever = dictionaryDataRetriever;
        }

        public void BuildCard(Card card)
        {
            //card.LexicalCategory = word.LexicalCategory;
            card.Definition = _dictionaryDataRetriever.GetDefinition(card);
            card.Examples = _dictionaryDataRetriever.GetExamples(card);
            (card.AudioFileName, card.AudioFileData) = _dictionaryDataRetriever.GetAudioFile(card);
        }
    }
}
