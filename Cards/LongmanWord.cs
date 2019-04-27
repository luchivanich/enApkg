using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cards
{
    // TODO: Rename. Rework
    public class LongmanWord
    {
        public string Word { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LexicalCategory? LexicalCategory { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Frequency Frequency { get; set; }
    }
}
