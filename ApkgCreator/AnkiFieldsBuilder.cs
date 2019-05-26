using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ApkgCreator.AdditionalModels;
using Cards;

namespace ApkgCreator
{
    public class AnkiFieldsBuilder : IAnkiFieldsBuilder
    {
        private const string FIELD_DELIMETER = "\u001f";
        private const string EXAMPLE_DELIMETER = "</br>";

        private Dictionary<string, Func<Card, string>> _fieldBuilders;

        public AnkiFieldsBuilder()
        {
            _fieldBuilders = new Dictionary<string, Func<Card, string>>();
            _fieldBuilders.Add("№", c => c.Id.ToString());
            _fieldBuilders.Add("English", EnglishFieldBuilder);
            _fieldBuilders.Add("Keyword", c => c.Word);
            _fieldBuilders.Add("Examples", ExamplesFieldBuilder);
        }

        public string BuildFieldsString(Card card, List<Fld> fields)
        {
            return string.Join(FIELD_DELIMETER, fields.Select(f => _fieldBuilders.ContainsKey(f.Name) ? _fieldBuilders[f.Name](card) : string.Empty).ToList());
        }

        private string EnglishFieldBuilder(Card card)
        {
            return $"{{{{c1::{card.Word}}}}} - {card.Definition}";
        }

        private string ExamplesFieldBuilder(Card card)
        {
            return card.Examples != null ? string.Join(EXAMPLE_DELIMETER, card.Examples.Select(e => Regex.Replace(e?.Value, card.Word, $"{{{{c1::{card.Word}}}}}", RegexOptions.IgnoreCase)).ToList()) : string.Empty;
        }
    }
}
