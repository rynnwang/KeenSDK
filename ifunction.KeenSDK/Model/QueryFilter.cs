using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ifunction.KeenSDK.Model
{
    /// <summary>
    /// Represents a filter that can be applied to a query.
    /// Because not all filter operators make sense for the different property data types, only certain operators are valid for each data type.
    /// </summary>
    public sealed class QueryFilter
    {
        #region Classes

        /// <summary>
        /// Class OperatorConverter.
        /// </summary>
        public class OperatorConverter : JsonConverter
        {
            /// <summary>
            /// Determines whether this instance can convert the specified object type.
            /// </summary>
            /// <param name="objectType">Type of the object.</param>
            /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, 
            /// <c>false</c>.</returns>
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(FilterOperator));
            }

            /// <summary>
            /// Reads the JSON representation of the object.
            /// </summary>
            /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="existingValue">The existing value of object being read.</param>
            /// <param name="serializer">The calling serializer.</param>
            /// <returns>The object value.</returns>
            /// <exception cref="System.NotImplementedException"></exception>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                // Don't need deserialization
                throw new NotImplementedException();
            }

            /// <summary>
            /// Writes the JSON representation of the object.
            /// </summary>
            /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
            /// <param name="value">The value.</param>
            /// <param name="serializer">The calling serializer.</param>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }

        /// <summary>
        /// Class FilterOperator. This class cannot be inherited.
        /// </summary>
        [JsonConverter(typeof(OperatorConverter))]
        public sealed class FilterOperator
        {
            /// <summary>
            /// The _value
            /// </summary>
            private readonly string _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="FilterOperator"/> class.
            /// </summary>
            /// <param name="value">The value.</param>
            private FilterOperator(string value) { _value = value; }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString() { return _value; }

            /// <summary>
            /// Performs an implicit conversion from <see cref="FilterOperator"/> to <see cref="System.String"/>.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator string(FilterOperator value) { return value.ToString(); }

            /// <summary>
            /// Equal to.
            /// <para>Use with string, number, boolean</para>
            /// </summary>
            /// <value>The equal.</value>
            public static FilterOperator Equal { get { return new FilterOperator("eq"); } }

            /// <summary>
            /// Not equal to.
            /// <para>Use with string, number</para>
            /// </summary>
            /// <value>The not equal.</value>
            public static FilterOperator NotEqual { get { return new FilterOperator("ne"); } }

            /// <summary>
            /// Less than.
            /// <para>Use with string, number</para>
            /// </summary>
            /// <value>The less than.</value>
            public static FilterOperator LessThan { get { return new FilterOperator("lt"); } }

            /// <summary>
            /// Less than or equal to.
            /// <para>Use with number</para>
            /// </summary>
            /// <value>The less than or equal.</value>
            public static FilterOperator LessThanOrEqual { get { return new FilterOperator("lte"); } }

            /// <summary>
            /// Greater than.
            /// <para>Use with string, number</para>
            /// </summary>
            /// <value>The greater than.</value>
            public static FilterOperator GreaterThan { get { return new FilterOperator("gt"); } }

            /// <summary>
            /// Greater than or equal to.
            /// <para>Use with number</para>
            /// </summary>
            /// <value>The greater than or equal.</value>
            public static FilterOperator GreaterThanOrEqual { get { return new FilterOperator("gte"); } }

            /// <summary>
            /// Whether a specific property exists on an event record.
            /// The Value property must be set to "true" or "false"
            /// <para>Use with string, number, boolean</para>
            /// </summary>
            /// <value>The exists.</value>
            public static FilterOperator Exists { get { return new FilterOperator("exists"); } }

            /// <summary>
            /// Whether the property value is in a give set of values.
            /// The Value property must be a JSON array of values, e.g.: "[1,2,4,5]"
            /// <para>Use with string, number, boolean</para>
            /// </summary>
            /// <value>The in.</value>
            public static FilterOperator In { get { return new FilterOperator("in"); } }

            /// <summary>
            /// Whether the property value contains the give set of characters.
            /// <para>Use with strings</para>
            /// </summary>
            /// <value>The contains.</value>
            public static FilterOperator Contains { get { return new FilterOperator("contains"); } }

            /// <summary>
            /// Used to select events within a certain radius of the provided geo coordinate.
            /// <para>Use with geo analysis</para>
            /// </summary>
            /// <value>The within.</value>
            public static FilterOperator Within { get { return new FilterOperator("within"); } }
        }

        /// <summary>
        /// Class GeoValue.
        /// </summary>
        public class GeoValue
        {
            /// <summary>
            /// Gets the coordinates.
            /// </summary>
            /// <value>The coordinates.</value>
            [JsonProperty(PropertyName = "coordinates")]
            public double[] Coordinates { get; private set; }

            /// <summary>
            /// Gets the maximum distance miles.
            /// </summary>
            /// <value>The maximum distance miles.</value>
            [JsonProperty(PropertyName = "max_distance_miles")]
            public double MaxDistanceMiles { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="GeoValue"/> class.
            /// </summary>
            /// <param name="longitude">The longitude.</param>
            /// <param name="latitude">The latitude.</param>
            /// <param name="maxDistanceMiles">The maximum distance miles.</param>
            public GeoValue(double longitude, double latitude, double maxDistanceMiles)
            {
                Coordinates = new double[] { longitude, latitude };
                MaxDistanceMiles = maxDistanceMiles;
            }
        }

        #endregion

        /// <summary>
        /// The name of the property on which to filter
        /// </summary>
        /// <value>The name of the property.</value>
        [JsonProperty(PropertyName = "property_name")]
        public string PropertyName { get; private set; }

        /// <summary>
        /// The filter operator to use
        /// </summary>
        /// <value>The operator.</value>
        [JsonProperty(PropertyName = "operator")]
        public FilterOperator Operator { get; private set; }

        /// <summary>
        /// The value to compare to the property specified in PropertyName
        /// </summary>
        /// <value>The value.</value>
        [JsonProperty(PropertyName = "property_value")]
        public object Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFilter"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="filterOperator">The op.</param>
        /// <param name="value">The value.</param>
        public QueryFilter(string property, FilterOperator filterOperator, object value)
        {
            property.CheckNullOrEmptyString("property");
            value.CheckNullObject("value");

            this.PropertyName = property;
            this.Operator = filterOperator;
            this.Value = value;
        }
    }
}
