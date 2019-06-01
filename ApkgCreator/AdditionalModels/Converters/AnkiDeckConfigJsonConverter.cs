using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApkgCreator.AdditionalModels.Converters
{
    class AnkiDeckConfigJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AnkiDeckConfig);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var deckConfig = jo.First.First.ToObject<DeckConfig>();
            return new AnkiDeckConfig { DeckConfig = deckConfig };
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue is AnkiDeckConfig ankiDeckConfig)
            {
                var jo = new JObject();
                jo.Add(ankiDeckConfig.DeckConfig.Id.ToString(), JToken.FromObject(ankiDeckConfig.DeckConfig));
                jo.WriteTo(writer);
                return;
            }
        }
    }
}
