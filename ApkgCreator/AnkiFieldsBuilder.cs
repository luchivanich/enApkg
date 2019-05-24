using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ApkgCreator.AdditionalModels;
using Cards;

namespace ApkgCreator
{
    public class AnkiFieldsBuilder : IAnkiFieldsBuilder
    {
        public string BuildFieldsString(Card card, List<Fld> fields)
        {
            var definition = " {{c1::" + card.Word + "}} - " + card.Definition;
            var examples = card.Examples?.Select(e => Regex.Replace(e?.Value, card.Word, "{{c1::" + card.Word + "}}", RegexOptions.IgnoreCase).ToString()).Aggregate(string.Empty, (current, next) => current + ";" + next);
            return $"{card.Id.ToString()}\u001f{string.Empty}\u001f{definition}\u001f{card.Word}\u001f{string.Empty}\u001f{string.Empty}\u001f{string.Empty}\u001f{examples}";
        }
    }
}
