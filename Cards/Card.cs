using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cards
{
    public class Card : IWord
    {
        public int Id { get; set; }

        public string Definition { get; set; }

        public List<string> Examples { get; set; }

        public byte[] AudioFileData { get; set; }

        public string AudioFileName { get; set; }

        public string Translation { get; set; }

        public string Word { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LexicalCategory? LexicalCategory { get; set; }
    }
}
