﻿using System;
using System.Collections.Generic;
using System.Text;
using ifunction.Analytic.Model;
using ifunction.KeenSDK.Core;
using ifunction.KeenSDK.Model;
using ifunction.Model;
using Newtonsoft.Json.Linq;

namespace ifunction.KeenSDK
{
    /// <summary>
    /// Class Extension.
    /// </summary>
    internal static class Extension
    {
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="queryType">Type of the query.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public static string ToValueString(this QueryType queryType)
        {
            string enumValue;

            switch (queryType)
            {
                case QueryType.CountUnique:
                    enumValue = "count_unique";
                    break;
                case QueryType.SelectUnique:
                    enumValue = "select_unique";
                    break;
                default:
                    enumValue = queryType.ToString().ToLowerInvariant();
                    break;
            }

            return enumValue;
        }

        /// <summary>
        /// Unifies the value.
        /// </summary>
        /// <param name="eventLogBase">The event log base.</param>
        public static void UnifyValue(this ApiEventLogBase eventLogBase)
        {
            if (eventLogBase == null) return;

            if (!string.IsNullOrEmpty(eventLogBase.ClientIdentifier))
            {
                eventLogBase.ClientIdentifier = eventLogBase.ClientIdentifier.ToLowerInvariant();
            }

            if (!string.IsNullOrEmpty(eventLogBase.HttpMethod))
            {
                eventLogBase.HttpMethod = eventLogBase.HttpMethod.ToUpperInvariant();
            }

            if (!string.IsNullOrEmpty(eventLogBase.ResourceName))
            {
                eventLogBase.ResourceName = eventLogBase.ResourceName.ToLowerInvariant();
            }

            if (!string.IsNullOrEmpty(eventLogBase.ResourceEntityKey))
            {
                eventLogBase.ResourceEntityKey = eventLogBase.ResourceEntityKey.ToLowerInvariant();
            }

            if (!string.IsNullOrEmpty(eventLogBase.ResourceAction))
            {
                eventLogBase.ResourceAction = eventLogBase.ResourceAction.ToLowerInvariant();
            }

            if (!string.IsNullOrEmpty(eventLogBase.ServerIdentifier))
            {
                eventLogBase.ServerIdentifier = eventLogBase.ServerIdentifier.ToLowerInvariant();
            }

            if (!string.IsNullOrEmpty(eventLogBase.ServiceIdentifier))
            {
                eventLogBase.ServiceIdentifier = eventLogBase.ServiceIdentifier.ToLowerInvariant();
            }

            if (!string.IsNullOrEmpty(eventLogBase.ModuleName))
            {
                eventLogBase.ModuleName = eventLogBase.ModuleName.ToLowerInvariant();
            }
        }

        #region JObject to return objects

        /// <summary>
        /// Queries the result to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jObject">The j object.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        public static IList<T> QueryResultToList<T>(this JObject jObject)
        {
            if (jObject != null)
            {
                return jObject.GetValue(KeenConstants.JsonNode_Result).ToObject<List<T>>();
            }

            return null;
        }

        /// <summary>
        /// Queries the result to groups.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jObject">The j object.</param>
        /// <param name="groupByNames">The group by names.</param>
        /// <param name="propertyMapping">The property mapping.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        public static IList<T> QueryResultToGroups<T>(this JObject jObject, IList<string> groupByNames, IDictionary<string, string> propertyMapping = null) where T : IGroupByResult, new()
        {
            IList<T> result = new List<T>();

            if (jObject != null && groupByNames != null && groupByNames.Count > 0)
            {
                var entityType = typeof(T);

                foreach (JObject item in jObject.Value<JArray>("result"))
                {
                    T entity = new T { Count = item.Value<int>("result") };

                    var displayNameBuilder = new StringBuilder();

                    foreach (var propertyName in groupByNames)
                    {
                        var value = item.Value<string>(propertyName);
                        var targetPropertyName = (propertyMapping != null && propertyMapping.ContainsKey(propertyName)) ? propertyMapping[propertyName] : propertyName;

                        var property = entityType.GetProperty(targetPropertyName);

                        if (property != null)
                        {
                            property.SetValue(entity, value);
                            displayNameBuilder.AppendFormat("{0}-", value);
                        }
                    }
                    entity.DisplayName = displayNameBuilder.ToString().TrimEnd('-');

                    result.Add(entity);
                }
            }

            return result;
        }

        /// <summary>
        /// Queries the result to interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jObject">The j object.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        public static IList<T> QueryResultToInterval<T>(this JObject jObject, AxisTimeInterval interval, int? timeZone) where T : IAnalyticStatistic, new()
        {
            IList<T> result = new List<T>();

            if (jObject != null && interval != null)
            {
                foreach (JObject item in jObject.Value<JArray>("result"))
                {
                    result.Add(new T
                    {
                        Count = item.Value<int>("value"),
                        StampIdentifier = item.Value<JObject>("timeframe").Value<DateTime>("start").ToDisplayName(interval, timeZone)
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Queries the result to interval groups.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jObject">The j object.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="groupByNames">The group by names.</param>
        /// <param name="propertyMapping">The property mapping.</param>
        /// <param name="getGroupName">Name of the get group.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        public static IList<T> QueryResultToIntervalGroups<T>(this JObject jObject, AxisTimeInterval interval, IList<string> groupByNames, IDictionary<string, string> propertyMapping = null, GetGroupName getGroupName = null) where T : IGroupByResult, new()
        {
            IList<T> result = new List<T>();

            if (jObject != null && groupByNames != null && groupByNames.Count > 0)
            {
                var entityType = typeof(T);

                foreach (JObject item in jObject.Value<JArray>("result"))
                {
                    var stampIdentifier =
                        item.Value<JObject>("timeframe")
                            .Value<DateTime>("start")
                            .ToString(interval.IntervalToDateTimeFormat());

                    foreach (JObject group in item.Value<JArray>("value"))
                    {
                        T entity = new T()
                        {
                            StampIdentifier = stampIdentifier,
                            Count = group.Value<int>("result")
                        };

                        var displayNameBuilder = new StringBuilder();
                        foreach (var propertyName in groupByNames)
                        {
                            var value = group.Value<string>(propertyName);
                            var targetPropertyName = (propertyMapping != null && propertyMapping.ContainsKey(propertyName)) ? propertyMapping[propertyName] : propertyName;

                            var property = entityType.GetProperty(targetPropertyName);

                            if (property != null)
                            {
                                property.SetValue(entity, value);
                                displayNameBuilder.AppendFormat("{0}-", value);
                            }
                        }

                        entity.DisplayName = displayNameBuilder.ToString().TrimEnd('-');
                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Queries the result to interval groups.
        /// </summary>
        /// <param name="jObject">The j object.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="groupByNames">The group by names.</param>
        /// <param name="propertyMapping">The property mapping.</param>
        /// <param name="getGroupName">Name of the get group.</param>
        /// <returns>GroupCollection&lt;AnalyticStatistic&gt;.</returns>
        public static GroupCollection<AnalyticStatistic> QueryResultToIntervalGroups(this JObject jObject, AxisTimeInterval interval, IList<string> groupByNames, IDictionary<string, string> propertyMapping = null, GetGroupName getGroupName = null)
        {
            var result = new GroupCollection<AnalyticStatistic>();

            if (jObject != null && groupByNames != null && groupByNames.Count > 0)
            {
                foreach (JObject item in jObject.Value<JArray>("result"))
                {
                    var stampIdentifier =
                        item.Value<JObject>("timeframe")
                            .Value<DateTime>("start")
                            .ToString(interval.IntervalToDateTimeFormat());

                    foreach (JObject group in item.Value<JArray>("value"))
                    {
                        var entity = new AnalyticStatistic()
                        {
                            StampIdentifier = stampIdentifier,
                            Count = group.Value<int>("result")
                        };

                        var groupName = getGroupName == null
                            ? DefaultGetGroupName(groupByNames, group)
                            : getGroupName(groupByNames, group);

                        result.Add(groupName, entity);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Defaults the name of the get group.
        /// </summary>
        /// <param name="groupByNames">The group by names.</param>
        /// <param name="jObject">The j object.</param>
        /// <returns>System.String.</returns>
        private static string DefaultGetGroupName(IList<string> groupByNames, JObject jObject)
        {
            var displayNameBuilder = new StringBuilder();

            if (groupByNames != null && jObject != null)
            {
                foreach (var propertyName in groupByNames)
                {
                    var value = jObject.Value<string>(propertyName);
                    displayNameBuilder.AppendFormat("{0}-", value);
                }
            }

            return displayNameBuilder.ToString().TrimEnd('-');
        }

        /// <summary>
        /// Intervals to date time format.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <returns>System.String.</returns>
        private static string IntervalToDateTimeFormat(this AxisTimeInterval interval)
        {
            if (interval == null) return string.Empty;

            switch (interval.Unit)
            {
                case TimeUnit.Day:
                case TimeUnit.Week:
                    return "yyyy-MM-dd";
                case TimeUnit.Hour:
                    return "yyyy-MM-dd HH:00";
                case TimeUnit.Minute:
                    return "yyyy-MM-dd-HH-mm";
                case TimeUnit.Month:
                    return "yyyy-MM";
                case TimeUnit.Year:
                    return "yyyy";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Intervals to date time format.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>System.String.</returns>
        private static string ToDisplayName(this DateTime dateTime, AxisTimeInterval interval, int? timeZone)
        {
            return dateTime.AddSeconds(timeZone ?? 0).ToString(interval.IntervalToDateTimeFormat());
        }

        #endregion
    }
}
