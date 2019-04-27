using System.Linq;

namespace Cards
{
    public class CardBuilder : ICardBuilder
    {
        private IDefinitionRetriever _definitionRetriever;
        private IExamplesRetriever _examplesRetriever;

        public CardBuilder(IDefinitionRetriever definitionRetriever, IExamplesRetriever examplesRetriver)
        {
            _definitionRetriever = definitionRetriever;
            _examplesRetriever = examplesRetriver;
        }

        public Card BuildNewCard(LongmanWord word)
        {
            var card = new Card();
            card.Word = word.Word;
            card.LexicalCategory = word.LexicalCategory;
            card.Frequency = word.Frequency;
            RetrieveCardData(card, word);
            return card;
        }

        public void BuildCard(Card card, LongmanWord word)
        {
            RetrieveCardData(card, word);
        }

        private void RetrieveCardData(Card card, LongmanWord word)
        {
            card.Definition = _definitionRetriever.GetDefinition(word);
            card.Examples = _examplesRetriever.GetExamples(word)?.Select(i => new Example { Value = i }).ToList();
        }
    }
}
