using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ifunction.KeenSDK.Model
{
    /// <summary>
    /// Class QueryTypeJsonConverter.
    /// </summary>
    public class QueryTypeJsonConverter : Newtonsoft.Json.JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, 
        /// <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(QueryType));
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Enum.Parse(objectType, existingValue.ToString().Replace(" ", ""), true);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string enumValue = string.Empty;

            switch ((QueryType)value)
            {
                case QueryType.CountUnique:
                    enumValue = "count_unique";
                    break;
                case QueryType.SelectUnique:
                    enumValue = "select_unique";
                    break;
                default:
                    ((QueryType)value).ToString().ToLowerInvariant();
                    break;
            }

            writer.WriteValue(((QueryType)value).ToValueString());
        }
    }
}
