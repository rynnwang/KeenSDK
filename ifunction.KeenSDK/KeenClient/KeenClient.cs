using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ifunction.ExceptionSystem;
using ifunction.KeenSDK.Core.AddOns;
using ifunction.KeenSDK.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ifunction.KeenSDK.Core
{
    /// <summary>
    /// Class KeenClient.
    /// </summary>
    public partial class KeenClient
    {
        /// <summary>
        /// The valid collection names
        /// </summary>
        private static HashSet<string> validCollectionNames = new HashSet<string>();

        #region Property

        /// <summary>
        /// Gets or sets the keen base URL.
        /// </summary>
        /// <value>The keen base URL.</value>
        public Uri KeenBaseUrl { get; protected set; }

        /// <summary>
        /// The Project ID, identifying the data silo to be accessed.
        /// </summary>
        public string ProjectId { get; protected set; }

        /// <summary>
        /// The Master API key, required for getting a collection schema
        /// or deleting the entire event collection.
        /// </summary>
        public string MasterKey { get; protected set; }

        /// <summary>
        /// The Write API key, required for inserting events.
        /// </summary>
        public string WriteKey { get; protected set; }

        /// <summary>
        /// The Read API key, used with query requests.
        /// </summary>
        public string ReadKey { get; protected set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="KeenClient" /> class.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="masterKey">The master key.</param>
        /// <param name="readKey">The read key.</param>
        /// <param name="writeKey">The write key.</param>
        /// <param name="keenBaseUrl">The keen base URL.</param>
        public KeenClient(string projectId, string masterKey = null, string readKey = null, string writeKey = null, Uri keenBaseUrl = null)
        {
            this.ProjectId = projectId;
            this.MasterKey = masterKey;
            this.ReadKey = readKey;
            this.WriteKey = writeKey;
            this.KeenBaseUrl = KeenBaseUrl == null ? new Uri(KeenConstants.DefaultBaseUrl) : keenBaseUrl;
        }

        #region Protected methods

        /// <summary>
        /// Generates the URL.
        /// </summary>
        /// <param name="module">The module.<example>queries</example></param>
        /// <param name="feature">The feature.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        protected HttpWebRequest GenerateUrl(string httpMethod, string module, string feature = null, Dictionary<string, string> parameters = null)
        {
            var queryString = parameters == null ? string.Empty : string.Join("&", from p in parameters.Keys
                                                                                   where !string.IsNullOrEmpty(parameters[p])
                                                                                   select string.Format("{0}={1}", p, Uri.EscapeDataString(parameters[p])));
            return GenerateUrl(httpMethod, this.KeenBaseUrl, this.ProjectId, module, feature, queryString);
        }

        /// <summary>
        /// Generates the URL.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="module">The module.</param>
        /// <param name="feature">The feature.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>System.String.</returns>
        protected static HttpWebRequest GenerateUrl(string httpMethod, Uri baseUrl, string projectId, string module, string feature, string queryString)
        {
            return (string.Format("{0}projects/{1}/{2}/{3}", baseUrl, projectId, module, feature).TrimEnd('/') + (string.IsNullOrWhiteSpace(queryString) ? string.Empty : ("?" + queryString))).CreateHttpWebRequest(httpMethod);
        }

        /// <summary>
        /// Apply the collection name restrictions. Throws KeenException with an
        /// explanation if a collection name is unacceptable.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        protected static void ValidateEventCollectionName(string collectionName)
        {
            // Avoid cost of re-checking collection names that have already been validated.
            // There is a race condition here, but it's harmless and does not justify the
            // overhead of synchronization.
            if (validCollectionNames.Contains(collectionName))
            {
                return;
            }

            collectionName.CheckEmptyString("collectionName");
            if (collectionName.Length > 64 || new Regex("[^\x00-\x7F]").Match(collectionName).Success
                || collectionName.Contains("$") || collectionName.StartsWith("_"))
            {
                throw new Exception("Event collection name should follows those requirements: length <= 64 characters; Ascii characters only; not contain $; not begin with _.");
            }

            validCollectionNames.Add(collectionName);
        }

        /// <summary>
        /// Gets the HTTP response.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Newtonsoft.Json.Linq.JObject.</returns>
        protected static JObject ExecuteHttpApi(HttpWebRequest request)
        {
            request.CheckNullObject("request");

            HttpStatusCode statusCode;
            var response = request.ReadResponseAsText(Encoding.UTF8, out statusCode);

            if (((int)statusCode).ToString().StartsWith("2"))
            {
                var jObject = JObject.Parse(response);
                CheckApiErrorCode(request.RequestUri.ToString(), jObject);

                return jObject;
            }
            else
            {
                throw new ApplicationException(response);
            }
        }

        /// <summary>
        /// Checks the API error code.
        /// </summary>
        /// <param name="operationName">Name of the operation.</param>
        /// <param name="jObject">The jobject.</param>
        protected static void CheckApiErrorCode(string operationName, JObject jObject)
        {
            var errorCode = jObject.Value<string>("error_code");

            if (!string.IsNullOrWhiteSpace(errorCode))
            {
                var message = jObject.Value<string>("message");

                throw new OperationFailureException(operationName, null, new { message, errorCode });
            }
        }

        /// <summary>
        /// Sets the basic authentication.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="authenticationKey">The authentication key.</param>
        protected static void SetBasicAuthentication(HttpWebRequest request, string authenticationKey)
        {
            if (request != null && !string.IsNullOrWhiteSpace(authenticationKey))
            {
                request.Headers.Add("Authorization", authenticationKey);
            }
        }

        /// <summary>
        /// Convert a user-supplied object to a JObject that can be sent to the Keen.IO API.
        /// This writes any global properties to the object and records the time.
        /// </summary>
        /// <param name="eventInfo">The event information.</param>
        /// <param name="addOnCollection">The add on collection.</param>
        /// <returns>JObject.</returns>
        protected JObject PrepareUserObject(object eventInfo, EventAddOnCollection addOnCollection)
        {
            var jEvent = JObject.FromObject(eventInfo);

            // Ensure this event has a 'keen' object of the correct type
            if (null == jEvent.Property("keen"))
            {
                jEvent.Add("keen", new JObject());
            }
            else if (jEvent.Property("keen").Value.GetType() != typeof(JObject))
            {
                throw new InvalidObjectException("keen");
            }

            JObject keen = ((JObject)jEvent.Property("keen").Value);

            // Set add-on nodes in JSON
            if (addOnCollection != null && addOnCollection.Count > 0)
            {
                var addOns = new List<object>();

                foreach (var one in addOnCollection)
                {
                    addOns.Add(JToken.FromObject(one));
                }
                keen.Add("addons", new JArray(addOns.ToArray()));
            }

            // Set the keen.timestamp if it has not already been set
            if (null == keen.Property("timestamp"))
            {
                keen.Add("timestamp", DateTime.UtcNow);
            }

            return jEvent;
        }

        /// <summary>
        /// Creates the criteria data.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="timeFrame">The time frame.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="groupByNames">The group by names.</param>
        /// <param name="analysisParameters">The analysis parameters.</param>
        /// <param name="steps">The steps.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="targetProperty">The target property.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        protected Dictionary<string, string> CreateCriteriaData(
            string collectionName,
            QueryTimeFrame timeFrame = null,
            IList<QueryFilter> filters = null,
            QueryInterval interval = null,
            IList<string> groupByNames = null,
            IList<MultiAnalysisParameter> analysisParameters = null,
             IList<FunnelStep> steps = null,
            int? timeZone = null,
            string targetProperty = null
            )
        {
            collectionName.CheckNullObject("collectionName");

            var parameters = new Dictionary<string, string> { { KeenConstants.QueryEventCollection, collectionName } };

            if (timeFrame != null)
            {
                parameters.Add(KeenConstants.QueryTimeFrame, timeFrame.ToSafeString());
            }

            if (timeZone != null)
            {
                parameters.Add(KeenConstants.QueryTimezone, timeZone.ToString());
            }

            if (filters != null && filters.Count > 0)
            {
                parameters.Add(KeenConstants.QueryFilters, JsonConvert.SerializeObject(filters));
            }

            if (interval != null)
            {
                parameters.Add(KeenConstants.QueryInterval, interval.ToString());
            }

            if (steps != null)
            {
                var jsonObject = steps.Select(i => JObject.FromObject(i));
                var stepsJson = new JArray(jsonObject).ToString();
                parameters.Add(KeenConstants.QuerySteps, stepsJson);
            }

            if (groupByNames != null && groupByNames.Count > 0)
            {
                if (groupByNames.Count > 1)
                {
                    var groupNames = new List<string>();
                    foreach (var one in groupByNames)
                    {
                        groupNames.Add(one);
                    }
                    parameters.Add(KeenConstants.QueryGroupBy, JsonConvert.SerializeObject(groupNames));
                }
                else
                {
                    parameters.Add(KeenConstants.QueryGroupBy, groupByNames[0].ToSafeString());
                }
            }

            if (analysisParameters != null && analysisParameters.Count > 0)
            {
                parameters.Add(KeenConstants.QueryAnalyses, JsonConvert.SerializeObject(new JObject(analysisParameters), Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            }

            if (!string.IsNullOrWhiteSpace(targetProperty))
            {
                parameters.Add(KeenConstants.QueryTargetProperty, targetProperty);
            }

            return parameters;
        }

        #endregion

        #region Collection

        /// <summary>
        /// Delete the specified collection. Deletion may be denied for collections with many events.
        /// Master API key is required.
        /// </summary>
        /// <param name="collectionName">Name of collection to delete.</param>
        public void DeleteCollection(string collectionName)
        {
            ValidateEventCollectionName(collectionName);
            this.MasterKey.CheckNullObject("MasterKey");

            var request = this.GenerateUrl("DELETE", KeenConstants.EventsResource, collectionName);
            SetBasicAuthentication(request, this.MasterKey);

            ExecuteHttpApi(request);
        }

        /// <summary>
        /// Return schema information for all the event collections in this project.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>Task&lt;JArray&gt;.</returns>
        public JObject GetSchema(string collectionName = null)
        {
            ValidateEventCollectionName(collectionName);
            this.MasterKey.CheckNullObject("MasterKey");

            var request = this.GenerateUrl("GET", KeenConstants.EventsResource, collectionName);
            SetBasicAuthentication(request, this.MasterKey);

            return ExecuteHttpApi(request);
        }

        #endregion

        #region AddEvent

        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="eventInfo">The event information.</param>
        /// <param name="addOn">The add on.</param>
        public void AddEvent(string collection, object eventInfo, IEventAddOn addOn = null)
        {
            EventAddOnCollection addOnes = null;

            if (addOn != null)
            {
                addOnes = new EventAddOnCollection {addOn};
            }

            AddEvent(collection, eventInfo, addOnes);
        }

        /// <summary>
        /// Add a single event to the specified collection.
        /// </summary>
        /// <param name="collectionName">Collection name</param>
        /// <param name="eventInfo">An object representing the event to be added.</param>
        /// <param name="addOnCollection">The add on collection.</param>
        public void AddEvent(string collectionName, object eventInfo, EventAddOnCollection addOnCollection)
        {
            ValidateEventCollectionName(collectionName);

            eventInfo.CheckNullObject("eventInfo");
            this.WriteKey.CheckEmptyString("WriteKey");

            var eventObject = PrepareUserObject(eventInfo, addOnCollection);

            var request = this.GenerateUrl("POST", KeenConstants.EventsResource, collectionName);
            SetBasicAuthentication(request, this.WriteKey);
            request.FillData("POST", eventObject.ToString(), "application/json");
            ExecuteHttpApi(request);
        }

        #endregion

        /// <summary>
        /// Gets the available queries.
        /// </summary>
        /// <returns>IEnumerable&lt;KeyValuePair&lt;System.String, System.String&gt;&gt;.</returns>
        public IEnumerable<KeyValuePair<string, string>> GetAvailableQueries()
        {
            var request = this.GenerateUrl("GET", KeenConstants.QueriesResource);
            SetBasicAuthentication(request, this.MasterKey);

            var reply = ExecuteHttpApi(request);

            return from j in reply.Children()
                   let p = j as JProperty
                   where p != null
                   select new KeyValuePair<string, string>(p.Name, (string)p.Value);
        }

        /// <summary>
        /// Commons the query.
        /// </summary>
        /// <param name="queryType">Type of the query.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="timeFrame">The time frame.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="groupByNames">The group by names.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="timezone">The timezone.</param>
        /// <param name="targetProperty">The target property.</param>
        /// <returns>JObject.</returns>
        public JObject CommonQuery(QueryType queryType, string collectionName, QueryTimeFrame timeFrame = null, IList<QueryFilter> filters = null, IList<string> groupByNames = null, QueryInterval interval = null, int? timezone = null, string targetProperty = null)
        {
            collectionName.CheckEmptyString("collection");
            this.ReadKey.CheckEmptyString("ReadKey");

            var parameters = CreateCriteriaData(
              collectionName: collectionName,
              timeFrame: timeFrame,
              timeZone: timezone,
              filters: filters,
              interval: interval,
              groupByNames: groupByNames,
              targetProperty: targetProperty);

            var request = this.GenerateUrl("GET", KeenConstants.QueriesResource, queryType.ToValueString(), parameters);
            SetBasicAuthentication(request, this.ReadKey);

            return ExecuteHttpApi(request);
        }

        /// <summary>
        /// Countings the by group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="timeFrame">The time frame.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="groupByNames">The group by names.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="timezone">The timezone.</param>
        /// <param name="propertyMapping">The property mapping.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        public IList<T> CountingByGroup<T>(string collectionName, QueryTimeFrame timeFrame = null, IList<QueryFilter> filters = null, IList<string> groupByNames = null, QueryInterval interval = null, int? timezone = null, IDictionary<string, string> propertyMapping = null)
            where T : IGroupByResult, new()
        {
            var result = CommonQuery(QueryType.Count, collectionName, timeFrame, filters, groupByNames, interval, timezone, null);

            return result.QueryResultToGroups<T>(groupByNames, propertyMapping);
        }

        /// <summary>
        /// Queries the object.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="timeFrame">The time frame.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="timezone">The timezone.</param>
        /// <param name="countByLatest">The count by latest.</param>
        /// <returns>IList&lt;JObject&gt;.</returns>
        public IList<JObject> QueryObject(string collectionName, QueryTimeFrame timeFrame = null, IList<QueryFilter> filters = null, int? timezone = null, int countByLatest = 100)
        {
            collectionName.CheckEmptyString("collection");
            this.ReadKey.CheckEmptyString("ReadKey");

            var parameters = this.CreateCriteriaData(
             collectionName: collectionName,
             timeFrame: timeFrame,
             filters: filters,
             timeZone: timezone);

            parameters.Add(KeenConstants.QueryLatest, countByLatest > 0 ? countByLatest.ToString() : string.Empty);

            var request = this.GenerateUrl("GET", KeenConstants.QueriesResource, KeenConstants.QueryExtraction, parameters);
            SetBasicAuthentication(request, this.ReadKey);

            var jObject = ExecuteHttpApi(request);
            return new List<JObject>(from item in jObject.Value<JArray>(KeenConstants.JsonNode_Result) select (JObject)item);
        }

        /// <summary>
        /// Queries the object as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="timeFrame">The time frame.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="timezone">The timezone.</param>
        /// <param name="countByLatest">The count by latest.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        public IList<T> QueryObjectAs<T>(string collectionName, QueryTimeFrame timeFrame = null, IList<QueryFilter> filters = null, int? timezone = null, int countByLatest = 100)
        {
            collectionName.CheckEmptyString("collection");
            this.ReadKey.CheckEmptyString("ReadKey");

            var parameters = this.CreateCriteriaData(
             collectionName: collectionName,
             timeFrame: timeFrame,
             filters: filters,
             timeZone: timezone);

            parameters.Add(KeenConstants.QueryLatest, countByLatest > 0 ? countByLatest.ToString() : string.Empty);

            var request = this.GenerateUrl("GET", KeenConstants.QueriesResource, KeenConstants.QueryExtraction, parameters);
            SetBasicAuthentication(request, this.ReadKey);

            var jObject = ExecuteHttpApi(request);
            return jObject.QueryResultToList<T>();
        }

        /// <summary>
        /// Posts the saved query.
        /// </summary>
        /// <param name="savedQueryName">Name of the saved query.</param>
        /// <param name="queryType">Type of the query.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="timeFrame">The time frame.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="groupByNames">The group by names.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="timezone">The timezone.</param>
        /// <param name="targetProperty">The target property.</param>
        /// <returns>JObject.</returns>
        public JObject PostSavedQuery(string savedQueryName, QueryType queryType, string collectionName, QueryTimeFrame timeFrame = null, IList<QueryFilter> filters = null, IList<string> groupByNames = null, QueryInterval interval = null, int? timezone = null, string targetProperty = null)
        {
            savedQueryName.CheckEmptyString("savedQueryName");

            var savedQuery = new
            {
                analysis_type = queryType.ToValueString(),
                event_collection = collectionName,
                target_property = targetProperty,
                filters = filters,
                timeframe = timeFrame,
                timezone = timezone,
                interval = interval
            };

            var request = this.GenerateUrl("PUT", KeenConstants.SavedQueries, savedQueryName);
            SetBasicAuthentication(request, this.MasterKey);
            request.FillData("PUT", savedQuery.ToJson(), "application/json");

            return ExecuteHttpApi(request);
        }
    }
}
