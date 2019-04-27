using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApkgCreator
{
    [Table("revlog")]
    public class AnkiRevlog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; } // epoch-seconds timestamp of when you did the review

        [ForeignKey("cid")]
        public AnkiCard Card { get; set; } // cards.id

        [Column("usn")]
        public int Usn { get; set; } // all my reviews have -1

        [Column("ease")]
        public int Ease { get; set; } // which button you pushed to score your recall. 1(wrong), 2(hard), 3(ok), 4(easy)

        [Column("ivl")]
        public int Interval { get; set; } // interval

        [Column("lastIvl")]
        public int LastInterval { get; set; } // last interval

        [Column("factor")]
        public int Factor { get; set; } // factor

        [Column("time")]
        public int Time { get; set; } // how many milliseconds your review took, up to 60000 (60s)

        [Column("type")]
        public int Type { get; set; }
    }
}
