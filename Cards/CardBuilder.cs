using System.Linq;

namespace Cards
{
    public class CardBuilder : ICardBuilder
    {
        private IDefinitionRetriever _definitionRetriever { get; set; }
        private IExamplesRetriever _examplesRetriever { get; set; }

        public CardBuilder(IDefinitionRetriever definitionRetriever, IExamplesRetriever examplesRetriver)
        {
            _definitionRetriever = definitionRetriever;
            _examplesRetriever = examplesRetriver;
        }

        public Card BuildCard(LongmanWord word)
        {
            var card = new Card();
            card.Word = word.Word;
            card.LexicalCategory = word.LexicalCategory;
            card.Frequency = word.Frequency;
            card.Definition = _definitionRetriever.GetDefinition(word);
            card.Examples = _examplesRetriever.GetExamples(word)?.Select(i => new Example { Value = i }).ToList();
            return card;
        }
    }
}
