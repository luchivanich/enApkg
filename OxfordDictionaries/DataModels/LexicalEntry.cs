using System.ComponentModel.DataAnnotations;

namespace OxfordDictionaries.DataModels
{
    public class LexicalEntry
    {
        [Key]
        public int Id { get; set; }

        public string Word { get; set; }

        public string LexicalCategory { get; set; }

        public string OxfordDictionaryLexicalEntryV2Json { get; set; }
    }
}
