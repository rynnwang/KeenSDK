using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ifunction.KeenSDK.Model
{
    /// <summary>
    /// Class QueryType. This class cannot be inherited.
    /// </summary>
    [JsonConverter(typeof(QueryTypeJsonConverter))]
    public enum QueryType
    {
        Count = 0,
        CountUnique,
        Minimum,
        Maximum,
        Average,
        Sum,
        SelectUnique
    }
}
