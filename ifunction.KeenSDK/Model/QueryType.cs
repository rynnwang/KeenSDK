using Newtonsoft.Json;

namespace ifunction.KeenSDK.Model
{
    /// <summary>
    /// Class QueryType. This class cannot be inherited.
    /// </summary>
    [JsonConverter(typeof(QueryTypeJsonConverter))]
    public enum QueryType
    {
        /// <summary>
        /// Value indicating it is count
        /// </summary>
        Count = 0,
        /// <summary>
        /// Value indicating it is count unique
        /// </summary>
        CountUnique,
        /// <summary>
        /// Value indicating it is minimum
        /// </summary>
        Minimum,
        /// <summary>
        /// Value indicating it is maximum
        /// </summary>
        Maximum,
        /// <summary>
        /// Value indicating it is average
        /// </summary>
        Average,
        /// <summary>
        /// Value indicating it is sum
        /// </summary>
        Sum,
        /// <summary>
        /// Value indicating it is select unique
        /// </summary>
        SelectUnique
    }
}
