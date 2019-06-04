using System.Collections.Generic;
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
            (string word, string definition, List<string> examples, string fileName, byte[] fileData) result;
            result = _dictionaryDataRetriever.GetDictionaryData(card);

            if (!string.IsNullOrWhiteSpace(result.word))
            {
                card.Word = result.word;
            }

            if (string.IsNullOrEmpty(card.Definition))
            {
                card.Definition = result.definition;
            }

            if (card.Examples == null)
            {
                card.Examples = new List<string>();
            }
            card.Examples.AddRange(result.examples ?? new List<string>());
            card.Examples = card.Examples.Distinct().ToList();

            card.AudioFileName = result.fileName;
            card.AudioFileData = result.fileData;
        }
    }
}
