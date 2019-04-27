using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApkgCreator
{
    [Table("notes")]
    public class AnkiNote
    {
        [Key]
        [Column("id")]
        public int Id { get; set; } // epoch seconds of when the note was created
      
        [Column("guid")]
        public string Guid { get; set; } //globally unique id, almost certainly used for syncing

        [Column("mid")]
        public long ModleId { get; set; } //model id

        [Column("mod")]
        public int Modified { get; set; } //modified timestamp, epoch seconds

        [Column("usn")]
        public int Usn { get; set; } //-1 for all my notes

        [Column("tags")]
        public string Tags { get; set; } //space-separated string of tags.seems to include space at the beginning and end of the field, almost certainly for LIKE "% tag %" queries

        [Column("flds")]
        public string Fields { get; set; } //the values of the fields in this note.separated by 0x1f (31).

        [Column("sfld")]
        public int Sfld { get; set; } //the text of the first field, used for anki2's new (simplistic) uniqueness checking

        [Column("csum")]
        public int Csum { get; set; } //dunno.not a unique field, but very few repeats

        [Column("flags")]
        public int Flags { get; set; } //0 for all my notes

        [Column("data")]
        public string Data { get; set; } // empty string for all my notes
    }
}
