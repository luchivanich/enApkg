using Cards;

namespace ApkgCreator
{
    public class AnkiFieldsBuilder : IAnkiFieldsBuilder
    {
        public string BuildFields(Card card)
        {
            var definition = card.Definition + " - {{c1::" + card.Word + "}}";
            return $"{card.Id.ToString()}\u001f{string.Empty}\u001f{definition}\u001f{card.Word}\u001f{string.Empty}\u001f{string.Empty}\u001f{string.Empty}\u001f{string.Empty}";
        }
    }
}
