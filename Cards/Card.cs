using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cards
{
    public class Card : LongmanWord
    {
        [Key]
        public int Id { get; set; }

        public string Definition { get; set; }

        [ForeignKey("Examples")]
        public List<Example> Examples { get; set; }

        public string AudioFileUri { get; set; }

        //public byte[] Mp3File { get; set; }

        public string Translation { get; set; }
    }
}
