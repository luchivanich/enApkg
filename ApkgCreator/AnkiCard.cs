using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApkgCreator
{
    [Table("cards")]
    public class AnkiCard
    {
        [Key]
        [Column("id")]
        public int Id { get; set; } // the epoch milliseconds of when the card was created

        [ForeignKey("nid")]
        public AnkiNote Note { get; set; } // notes.id

        [Column("did")]
        public long DeckId { get; set; } // deck id (available in col table)

        [Column("ord")]
        public int Ordinal { get; set; } // ordinal, seems like. for when a model has multiple templates, or thereabouts

        [Column("mod")]
        public int ModifiedTime { get; set; } // modified time as epoch seconds

        [Column("usn")]
        public int Usn { get; set; } // "From the source code it appears Anki increments this number each time you synchronize with AnkiWeb and applies this number to the cards that were synchronized. My database is up to 1230 for the collection and my cards have various numbers up to 1229." -- contributed by Fletcher Moore

        [Column("type")]
        public int Type { get; set; } // in anki1, type was whether the card was suspended, etc. seems to be the same. values are 0 (suspended), 1 (maybe "learning"?), 2 (normal)

        [Column("queue")]
        public int Queue { get; set; } // "queue in the cards table refers to if the card is "new" = 0, "learning" = 1 or 3, "review" = 2 (I don't understand how a 3 occurs, but I have 3 notes out of 23,000 with this value.)" -- contributed by Fletcher Moore

        [Column("due")]
        public int Due { get; set; }

        [Column("ivl")]
        public int Interval { get; set; } // interval (used in SRS algorithm)

        [Column("factor")]
        public int Factor { get; set; } // factor (used in SRS algorithm)

        [Column("reps")]
        public int NumberOfReviews { get; set; } // number of reviews

        [Column("lapses")]
        public int Lapses { get; set; } // possibly the number of times the card went from a "was answered correctly" to "was answered incorrectly" state

        [Column("left")]
        public int Left { get; set; } // 0 for all my cards

        [Column("odue")]
        public int Odue { get; set; } // 0 for all my cards

        [Column("odid")]
        public int Odid { get; set; } // 0 for all my cards

        [Column("flags")]
        public int Flags { get; set; } // 0 for all my cards

        [Column("data")]
        public string Data { get; set; } // currently unused for decks imported from anki1. maybe extra data for plugins?
    }
}
