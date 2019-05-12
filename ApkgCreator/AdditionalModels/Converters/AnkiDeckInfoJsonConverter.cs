using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApkgCreator.AdditionalModels.Converters
{
    class AnkiDeckInfoJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?) || t == typeof(AnkiDeckInfo);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue is AnkiDeckInfo ankiDeckInfo)
            {
                var jo = new JObject();
                jo.Add(ankiDeckInfo.Deck.Id, JToken.FromObject(ankiDeckInfo.Deck));
                jo.WriteTo(writer);
                return;
            }

            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }
    }
}
