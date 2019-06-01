using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApkgCreator.AdditionalModels;
using ApkgCreator.AdditionalModels.Converters;
using Newtonsoft.Json;

namespace ApkgCreator.DataModels
{
    [Table("col")]
    public class AnkiCol
    {
        [Key]
        [Column("id")]
        public int Id { get; set; } // seems to be an autoincrement

        [Column("crt")]
        public long Created { get; set; } // there's the created timestamp

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
        public string AnkiColConfigJson { get; set; } // json blob of configuration

        [Column("models")]
        public string AnkiModelJson { get; set; } // json object with keys being ids(epoch ms), values being configuration

        [Column("decks")]
        public string AnkiDeckInfoJson { get; set; } // json object with keys being ids(epoch ms), values being configuration

        [Column("dconf")]
        public string AnkiDeckConfigJson { get; set; } // json object. deck configuration?

        [Column("tags")]
        public string Tags { get; set; } // a cache of tags used in this collection (probably for autocomplete etc)

        private AnkiModel _ankiModel;
        [NotMapped]
        public AnkiModel AnkiModel {
            get => _ankiModel;
            set
            {
                _ankiModel = value;
                AnkiModelJson = JsonConvert.SerializeObject(_ankiModel, new AnkiModelJsonConverter());
            }
        }

        private AnkiDeckInfo _ankiDeckInfo;
        [NotMapped]
        public AnkiDeckInfo AnkiDeckInfo
        {
            get => _ankiDeckInfo;
            set
            {
                _ankiDeckInfo = value;
                AnkiDeckInfoJson = JsonConvert.SerializeObject(_ankiDeckInfo, new AnkiDeckInfoJsonConverter());
            }
        }

        private AnkiColConfig _ankiColConfig;
        [NotMapped]
        public AnkiColConfig AnkiColConfig
        {
            get => _ankiColConfig;
            set
            {
                _ankiColConfig = value;
                AnkiColConfigJson = JsonConvert.SerializeObject(_ankiColConfig);
            }
        }

        private AnkiDeckConfig _ankiDeckConfig;
        [NotMapped]
        public AnkiDeckConfig AnkiDeckConfig
        {
            get => _ankiDeckConfig;
            set
            {
                _ankiDeckConfig = value;
                AnkiDeckConfigJson = JsonConvert.SerializeObject(_ankiDeckConfig, new AnkiDeckConfigJsonConverter());
            }
        }

        public void DeserializeProperties()
        {
            _ankiModel = JsonConvert.DeserializeObject<AnkiModel>(AnkiModelJson, new AnkiModelJsonConverter());
            _ankiDeckInfo = JsonConvert.DeserializeObject<AnkiDeckInfo>(AnkiDeckInfoJson, new AnkiDeckInfoJsonConverter());
            _ankiColConfig = JsonConvert.DeserializeObject<AnkiColConfig>(AnkiColConfigJson);
            _ankiDeckConfig = JsonConvert.DeserializeObject<AnkiDeckConfig>(AnkiDeckConfigJson, new AnkiDeckConfigJsonConverter());
        }
    }
}
