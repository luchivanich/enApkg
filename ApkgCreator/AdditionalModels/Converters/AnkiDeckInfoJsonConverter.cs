using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApkgCreator.AdditionalModels.Converters
{
    class AnkiDeckInfoJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AnkiDeckInfo);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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
        }
    }
}
