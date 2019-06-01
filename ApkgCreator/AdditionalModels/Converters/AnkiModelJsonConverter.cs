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
            var jo = JObject.Load(reader);
            var model = jo.First.First.ToObject<Model>();
            return new AnkiModel { Model = model };
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
