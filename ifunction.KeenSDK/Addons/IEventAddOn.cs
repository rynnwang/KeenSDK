using System.Collections.Generic;

namespace ifunction.KeenSDK.Core.AddOns
{
    /// <summary>
    /// Interface IEventAddOn is to achieve feature for add-ons.
    /// <remarks>
    /// https://keen.io/docs/data-collection/data-enrichment/
    /// </remarks>
    /// </summary>
    public interface IEventAddOn
    {
        /// <summary>
        /// Gets the add on identifier.
        /// </summary>
        /// <value>The add on identifier.</value>
        string AddOnIdentifier { get; }

        /// <summary>
        /// Gets the input parameters.
        /// </summary>
        /// <value>The input parameters.</value>
        Dictionary<string, string> InputParameters { get; }

        /// <summary>
        /// Gets the name of the output property.
        /// </summary>
        /// <value>The name of the output property.</value>
        string OutputPropertyName { get; }
    }
}
