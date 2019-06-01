using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ApkgCreator.AdditionalModels;
using ApkgCreator.DataModels;
using Cards;

namespace ApkgCreator
{
    public class AnkiFieldsBuilder : IAnkiFieldsBuilder
    {
        private const string FIELD_DELIMETER = "\u001f";

        private Dictionary<string, Func<Card, string>> _fieldBuilders;

        public AnkiFieldsBuilder()
        {
            _fieldBuilders = new Dictionary<string, Func<Card, string>>();
            _fieldBuilders.Add("Definition", DefinitionFieldBuilder);
            _fieldBuilders.Add("Keyword", c => c.Word);
            _fieldBuilders.Add("Sound", c => $"[sound:{c.AudioFileName}]");
            _fieldBuilders.Add("Examples", ExamplesFieldBuilder);
        }

        public Dictionary<string, string> BuildFieldsPairs(AnkiNote note, AnkiCol ankiCol)
        {
            var result = new Dictionary<string, string>();

            var fields = ankiCol.AnkiModel.Model.Flds.Select(f => f.Name).ToList();
            var values = note.Fields.Split(new string[1] { FIELD_DELIMETER }, StringSplitOptions.None);
            for (var i = 0; i < fields.Count(); i++)
            {
                result.Add(fields[i], values[i]);
            }
            return result;
        }

        public string BuildFieldsString(Card card, List<Fld> fields)
        {
            return string.Join(FIELD_DELIMETER, fields.Select(f => _fieldBuilders.ContainsKey(f.Name) ? _fieldBuilders[f.Name](card) : string.Empty).ToList());
        }

        private string DefinitionFieldBuilder(Card card)
        {
            return $"{{{{c1::{card.Word}}}}} - {card.Definition}";
        }

        private string ExamplesFieldBuilder(Card card)
        {
            if (card.Examples == null || card.Examples.Count == 0)
            {
                return string.Empty;
            }

            var result = string.Empty;
            var examplesTemplate = File.ReadAllText(@"Templates/Examples.htm");
            var exampleTemplate = File.ReadAllText(@"Templates/Example.htm");
            foreach (var example in card.Examples)
            {
                result += exampleTemplate.Replace("@Example", Regex.Replace(example, card.Word, $"{{{{c1::{card.Word}}}}}", RegexOptions.IgnoreCase));

            }
            result = examplesTemplate.Replace("@Examples", result);
            return result;
        }
    }
}
