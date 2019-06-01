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
            var word = string.Empty;
            (word, card.Definition, card.Examples, card.AudioFileName, card.AudioFileData) = _dictionaryDataRetriever.GetDictionaryData(card);
            if (!string.IsNullOrWhiteSpace(word))
            {
                card.Word = word;
            }
        }
    }
}
