using System.Collections.Generic;
using Newtonsoft.Json;

namespace ifunction.KeenSDK.Core.AddOns
{
    /// <summary>
    /// Class EventAddOn.
    /// <remarks>
    /// https://keen.io/docs/data-collection/data-enrichment/
    /// </remarks>
    /// </summary>
    public abstract class EventAddOn : IEventAddOn
    {
        /// <summary>
        /// Gets or sets the add on identifier.
        /// </summary>
        /// <value>The add on identifier.</value>
        [JsonProperty(PropertyName = "name")]
        public string AddOnIdentifier { get; protected set; }

        /// <summary>
        /// Gets or sets the input parameters.
        /// </summary>
        /// <value>The input parameters.</value>
        [JsonProperty(PropertyName = "input")]
        public Dictionary<string, string> InputParameters { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the output property.
        /// </summary>
        /// <value>The name of the output property.</value>
        [JsonProperty(PropertyName = "output")]
        public string OutputPropertyName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAddOn"/> class.
        /// </summary>
        /// <param name="addOnIdentifier">The add on identifier.</param>
        protected EventAddOn(string addOnIdentifier)
        {
            this.AddOnIdentifier = addOnIdentifier;
            this.InputParameters = new Dictionary<string, string>();
        }
    }
}
