using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApkgCreator.DataModels
{
    [Table("graves")]
    public class AnkiGraves
    {
        [Key]
        [Column("usn")]
        public int Usn { get; set; }

        [Column("oid")]
        public int Oid { get; set; }
        
        [Column("Type")]
        public int Type { get; set; }
    }
}
