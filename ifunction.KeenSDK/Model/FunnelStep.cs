using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ifunction.KeenSDK.Model
{
    /// <summary>
    /// Represents one step in a funnel query. See the Keen.IO API for details on how to perform a funnel query.
    /// </summary>
    public class FunnelStep
    {
        /// <summary>
        ///  The name of the event that defines the step.
        /// </summary>
        [JsonProperty(PropertyName = "event_collection")]
        public string EventCollection { get; set; }

        /// <summary>
        /// The name of the property that can be used as a unique identifier for a user (or any type of actor).
        /// </summary>
        [JsonProperty(PropertyName = "actor_property")]
        public string ActorProperty { get; set; }

        /// <summary>
        /// Filters are used to narrow the scope of events used in this step of the funnel.
        /// </summary>
        [JsonProperty(PropertyName = "filters", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<QueryFilter> Filters { get; set; }

        /// <summary>
        /// Window of time to use for the analysis. If not set, the timeFrame from the funnel will be inherited, if available.
        /// </summary>
        [JsonProperty(PropertyName = "timeFrame", NullValueHandling = NullValueHandling.Ignore)]
        public QueryTimeFrame TimeFrame { get; set; }

        /// <summary>
        /// Offset from UTC in seconds. If not set, the timezone from the funnel will be inherited, if available.
        /// Unit: second
        /// </summary>
        [JsonProperty(PropertyName = "timezone", NullValueHandling = NullValueHandling.Ignore)]
        public int TimeZone { get; set; }
    }
}
