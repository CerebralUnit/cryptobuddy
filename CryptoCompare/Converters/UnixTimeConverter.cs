using System;

using Newtonsoft.Json;

namespace CryptoCompare
{
    internal class UnixTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }
            return Convert.ToInt64(reader.Value).FromUnixTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var Parsed = DateTime.MinValue;

            DateTime.TryParse(value.ToString(), out Parsed);

           
            var epoc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var delta = Parsed.ToUniversalTime() - epoc;

            var ticks = (long)delta.TotalSeconds;

            writer.WriteValue(ticks);
        }
    }
}
