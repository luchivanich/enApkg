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

        [ForeignKey("Example")]
        public List<Example> Examples { get; set; }
    }
}
