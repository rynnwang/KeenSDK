
namespace ifunction.KeenSDK.Core.AddOns
{
    /// <summary>
    /// Class ReferrerParser.
    /// </summary>
    public class ReferrerParser : EventAddOn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferrerParser" /> class.
        /// </summary>
        /// <param name="referrerUrl">The referrer URL.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="outputPropertyName">Name of the output property.</param>
        public ReferrerParser(string referrerUrl = null, string pageUrl = null, string outputPropertyName = null)
            : base("keen:referrer_parser")
        {
            this.OutputPropertyName = outputPropertyName;
            this.InputParameters.Add("referrer_url", referrerUrl.ToSafeString());
            this.InputParameters.Add("page_url", pageUrl.ToSafeString());
        }
    }
}
