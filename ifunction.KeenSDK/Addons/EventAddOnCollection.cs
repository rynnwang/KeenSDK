using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace ifunction.KeenSDK.Core.AddOns
{
    /// <summary>
    /// Class EventAddOnCollection.
    /// </summary>
    public class EventAddOnCollection : Collection<IEventAddOn>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventAddOnCollection"/> class.
        /// </summary>
        public EventAddOnCollection()
            : base()
        {
        }

        /// <summary>
        /// To the j object.
        /// </summary>
        /// <returns>JObject.</returns>
        public JObject ToJObject()
        {
            return null;
        }
    }
}
