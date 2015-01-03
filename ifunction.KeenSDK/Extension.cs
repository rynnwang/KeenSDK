using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ifunction.KeenSDK.Core;
using ifunction.KeenSDK.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ifunction.KeenSDK
{
    /// <summary>
    /// Class Extension.
    /// </summary>
    internal static class Extension
    {
        /// <summary>
        /// To the safe string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        public static string ToSafeString(this object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        /// <summary>
        /// Tries the get int.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        public static int? TryGetInt(this string s)
        {
            int i;
            return int.TryParse(s, out i) ? (int?)i : null;
        }

        /// <summary>
        /// Tries the get double.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>System.Nullable&lt;System.Double&gt;.</returns>
        public static double? TryGetDouble(this string s)
        {
            double i;
            return double.TryParse(s, out i) ? (double?)i : null;
        }

        /// <summary>
        /// Checks the null object.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="objectIdentifier">The object identifier.</param>
        /// <exception cref="System.NullReferenceException"></exception>
        public static void CheckNullObject(this object anyObject, string objectIdentifier)
        {
            if (anyObject == null)
            {
                throw new NullReferenceException(string.Format("[{0}] is null.", objectIdentifier));
            }
        }

        /// <summary>
        /// Creates the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> CreateList<T>(this T anyObject)
        {
            List<T> result = new List<T>();

            if (anyObject != null)
            {
                result.Add(anyObject);
            }

            return result;
        }


        /// <summary>
        /// Checks the null or empty string.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="stringIdentifier">The string identifier.</param>
        /// <exception cref="System.NullReferenceException"></exception>
        public static void CheckNullOrEmptyString(this string anyString, string stringIdentifier)
        {
            if (string.IsNullOrWhiteSpace(anyString))
            {
                throw new NullReferenceException(string.Format("[{0}] is null or empty.", stringIdentifier));
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="queryType">Type of the query.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public static string ToValueString(this QueryType queryType)
        {
            string enumValue = string.Empty;

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
        /// <returns>IList&lt;T&gt;.</returns>
        public static IList<T> QueryResultToGroups<T>(this JObject jObject, IList<string> groupByNames) where T : IGroupByResult, new()
        {
            IList<T> result = new List<T>();

            if (jObject != null && groupByNames != null && groupByNames.Count > 0)
            {
                var entityType = typeof(T);

                foreach (JObject item in jObject.Value<JArray>("result"))
                {
                    T entity = new T();

                    foreach (var propertyName in groupByNames)
                    {
                        var value = item.Value<string>(propertyName);
                        var property = entityType.GetProperty(propertyName);

                        if (property != null)
                        {
                            property.SetValue(entity, value);
                        }
                    }

                    entity.Count = item.Value<int>("result");
                    result.Add(entity);
                }
            }

            return result;
        }

        #endregion
    }
}
