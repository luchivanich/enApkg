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
            (card.Word, card.Definition, card.Examples, card.AudioFileName, card.AudioFileData) = _dictionaryDataRetriever.GetDictionaryData(card);
        }
    }
}
