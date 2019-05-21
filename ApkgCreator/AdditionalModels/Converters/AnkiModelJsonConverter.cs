using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApkgCreator.AdditionalModels.Converters
{
    class AnkiModelJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AnkiModel);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue is AnkiModel ankiModel)
            {
                var jo = new JObject();
                jo.Add(ankiModel.Model.Id.ToString(), JToken.FromObject(ankiModel.Model));
                jo.WriteTo(writer);
                return;
            }
        }
    }
}
