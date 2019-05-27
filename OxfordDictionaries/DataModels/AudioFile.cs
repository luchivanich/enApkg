using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OxfordDictionaries.DataModels
{
    public class AudioFile
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        public string FileName { get; set; }

        public byte[] Data { get; set; }

        [ForeignKey("AudioFileId")]
        public List<LexicalEntry> LexicalEntries { get; set; }
    }
}
