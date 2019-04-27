using System.ComponentModel.DataAnnotations;

namespace OxfordDictionaries
{
    public class CachedItem
    {
        [Key]
        public int Id { get; set; }

        public string Word { get; set; }

        public string OxfordDictionaryEntityJson { get; set; }
    }
}
