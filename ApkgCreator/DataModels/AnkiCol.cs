using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApkgCreator.DataModels
{
    [Table("col")]
    public class AnkiCol
    {
        [Key]
        [Column("id")]
        public int Id { get; set; } // seems to be an autoincrement

        [Column("crt")]
        public int Created { get; set; } // there's the created timestamp

        [Column("mod")]
        public long Modified { get; set; } // last modified in milliseconds

        [Column("scm")]
        public long SchemaModeTime { get; set; } // a timestamp in milliseconds. "schema mod time" - contributed by Fletcher Moore

        [Column("ver")]
        public int Ver { get; set; } // version? I have "11"

        [Column("dty")]
        public int Dty { get; set; } // 0

        [Column("usn")]
        public int Usn { get; set; } // 0

        [Column("ls")]
        public int LastSyncTime { get; set; } // "last sync time" - contributed by Fletcher Moore

        [Column("conf")]
        public string Conf { get; set; } // json blob of configuration

        [Column("models")]
        public string Models { get; set; } // json object with keys being ids(epoch ms), values being configuration

        [Column("decks")]
        public string Decks { get; set; } // json object with keys being ids(epoch ms), values being configuration

        [Column("dconf")]
        public string DeckConfiguration { get; set; } // json object. deck configuration?

        [Column("tags")]
        public string Tags { get; set; } // a cache of tags used in this collection (probably for autocomplete etc)
    }
}
