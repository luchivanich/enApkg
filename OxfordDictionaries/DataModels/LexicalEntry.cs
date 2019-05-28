using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OxfordDictionaries.DataModels
{
    public class LexicalEntry
    {
        [Key]
        public int Id { get; set; }

        public string Word { get; set; }

        public string LexicalCategory { get; set; }

        public string OxfordDictionaryLexicalEntryV2Json { get; set; }

        public AudioFile AudioFile { get; set; }

        [NotMapped]
        public OxfordDictionaryLexicalEntryV2 OxfordDictionaryLexicalEntryV2 => JsonConvert.DeserializeObject<OxfordDictionaryLexicalEntryV2>(OxfordDictionaryLexicalEntryV2Json ?? string.Empty);
    }
}
