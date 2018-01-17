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
            if (String.IsNullOrWhiteSpace( reader.Value.ToString() ) )
            {
                

                return GetMin();
            }

            try
            {
               var val = Convert.ToInt64(reader.Value).FromUnixTime();
                return val;
            }
            catch
            {
                return GetMin();
            }
        }
         private object GetMin()
        {
            var Parsed = DateTime.MinValue;
            var epoc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var delta = Parsed.ToUniversalTime() - epoc;

            var ticks = (long)delta.TotalSeconds;

            return Convert.ToInt64(ticks).FromUnixTime();
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
