using Newtonsoft.Json;

namespace ifunction.KeenSDK.Core.AddOns
{
    /// <summary>
    /// Class UserAgentParser.
    /// </summary>
    public class UserAgentParser : EventAddOn
    {
        const string inputParameterName = "ua_string";

        /// <summary>
        /// Gets or sets the name of the input property.
        /// </summary>
        /// <value>The name of the input property.</value>
        [JsonIgnore]
        public string InputPropertyName
        {
            get
            {
                return this.InputParameters[inputParameterName];
            }
            set
            {
                this.InputParameters[inputParameterName] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentParser" /> class.
        /// </summary>
        /// <param name="inputPropertyName">Name of the input property.</param>
        /// <param name="outputPropertyName">Name of the output property.</param>
        public UserAgentParser(string inputPropertyName = null, string outputPropertyName = null)
            : base("keen:ua_parser")
        {
            this.OutputPropertyName = outputPropertyName;
            this.InputParameters.Add(inputParameterName, inputPropertyName.ToSafeString());
        }
    }
}
