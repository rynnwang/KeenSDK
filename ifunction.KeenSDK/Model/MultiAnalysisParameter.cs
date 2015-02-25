namespace ifunction.KeenSDK.Model
{
    /// <summary>
    /// Class MultiAnalysisParameter. This class cannot be inherited.
    /// </summary>
    public sealed class MultiAnalysisParameter
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label { get; private set; }

        /// <summary>
        /// Gets the type of the query.
        /// </summary>
        /// <value>The type of the query.</value>
        public QueryType QueryType { get; private set; }

        /// <summary>
        /// Gets the target property.
        /// </summary>
        /// <value>The target property.</value>
        public string TargetProperty { get; private set; }

        /// <summary>
        /// MultiAnalysisParam defines one kind of analysis to run in a MultiAnalysis request.
        /// </summary>
        /// <param name="label">A user defined string that acts as a name for the analysis.
        /// This will be returned in the results so the various analyses are easily identifiable.</param>
        /// <param name="queryType">The metric type.</param>
        /// <param name="targetProperty">The target property.</param>
        public MultiAnalysisParameter(string label, QueryType queryType, string targetProperty)
        {
            Label = label;
            QueryType = queryType;
            TargetProperty = targetProperty;
        }
    }
}
