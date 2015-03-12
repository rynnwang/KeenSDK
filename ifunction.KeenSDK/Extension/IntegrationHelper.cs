using System;
using System.Collections.Generic;
using ifunction.Analytic.Model;
using ifunction.ExceptionSystem;
using ifunction.KeenSDK.Model;

namespace ifunction.KeenSDK
{
    /// <summary>
    /// Class IntegrationHelper.
    /// </summary>
    public static class IntegrationHelper
    {
        #region public methods

        /// <summary>
        /// Times the zone name to offset second.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.Int32.</returns>
        public static int TimeZoneNameToOffsetSecond(string name)
        {
            var timeZoneInfo = string.IsNullOrWhiteSpace(name)
                ? null
                : TimeZoneInfo.FindSystemTimeZoneById(name);

            return timeZoneInfo != null ? (int)timeZoneInfo.BaseUtcOffset.TotalSeconds : 0;
        }

        /// <summary>
        /// To the query time frame.
        /// </summary>
        /// <param name="timeFrame">The time frame.</param>
        /// <returns>QueryTimeFrame.</returns>
        public static QueryTimeFrame ToQueryTimeFrame(ITimeFrame timeFrame)
        {
            return timeFrame == null
                ? null
                : ((timeFrame.FromStamp != null && timeFrame.ToStamp != null)
                        ? QueryTimeFrame.AbsoluteTimeFrame(timeFrame.FromStamp.Value, timeFrame.ToStamp.Value)
                        : QueryTimeFrame.ThisNDays((timeFrame.LastNDays.HasValue && timeFrame.LastNDays.Value > 0) ? timeFrame.LastNDays.Value : 7));
        }

        /// <summary>
        /// To the query filters.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;QueryFilter&gt;.</returns>
        public static List<QueryFilter> ToQueryFilters(ExceptionCriteria criteria)
        {
            var result = new List<QueryFilter>();

            if (criteria != null)
            {
                if (criteria.Key != null)
                {
                    result.Add(new QueryFilter("Key", QueryFilter.FilterOperator.Equal, criteria.Key));
                }
                else
                {
                    result.AddRange(ToQueryFilters(criteria as ExceptionBase));

                    if (criteria.FromStamp != null)
                    {
                        result.Add(new QueryFilter("CreatedStamp", QueryFilter.FilterOperator.GreaterThanOrEqual, criteria.FromStamp));
                    }

                    if (criteria.ToStamp != null)
                    {
                        result.Add(new QueryFilter("CreatedStamp", QueryFilter.FilterOperator.LessThan, criteria.ToStamp));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// To the query filters.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;QueryFilter&gt;.</returns>
        public static List<QueryFilter> ToQueryFilters(ExceptionBase criteria)
        {
            var result = new List<QueryFilter>();

            if (criteria == null) return result;

            if (criteria.Level != null)
            {
                result.Add(new QueryFilter("Level", QueryFilter.FilterOperator.Equal, criteria.Level));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Message))
            {
                result.Add(new QueryFilter("Message", QueryFilter.FilterOperator.Contains, criteria.Message));
            }

            if (!string.IsNullOrWhiteSpace(criteria.UserIdentifier))
            {
                result.Add(new QueryFilter("UserIdentifier", QueryFilter.FilterOperator.Equal, criteria.UserIdentifier));
            }

            if (criteria.ExceptionType != null)
            {
                result.Add(new QueryFilter("ExceptionType", QueryFilter.FilterOperator.Equal, criteria.ExceptionType));
            }

            if (!string.IsNullOrWhiteSpace(criteria.ServiceIdentifier))
            {
                result.Add(new QueryFilter("ServiceIdentifier", QueryFilter.FilterOperator.Equal, criteria.ServiceIdentifier));
            }

            if (!string.IsNullOrWhiteSpace(criteria.ServerIdentifier))
            {
                result.Add(new QueryFilter("ServerIdentifier", QueryFilter.FilterOperator.Equal, criteria.ServerIdentifier));
            }

            if (criteria.Code != null)
            {
                result.Add(new QueryFilter("Code.Major", QueryFilter.FilterOperator.Equal, (int)criteria.Code.Major));
                if (!string.IsNullOrWhiteSpace(criteria.Code.Minor))
                {
                    result.Add(new QueryFilter("Code.Minor", QueryFilter.FilterOperator.Equal, criteria.Code.Minor));
                }
            }

            if (!string.IsNullOrWhiteSpace(criteria.Source))
            {
                result.Add(new QueryFilter("Source", QueryFilter.FilterOperator.Contains, criteria.Source));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Data))
            {
                result.Add(new QueryFilter("Data", QueryFilter.FilterOperator.Contains, criteria.Data));
            }

            if (!string.IsNullOrWhiteSpace(criteria.TargetSite))
            {
                result.Add(new QueryFilter("TargetSite", QueryFilter.FilterOperator.Contains, criteria.TargetSite));
            }

            if (!string.IsNullOrWhiteSpace(criteria.StackTrace))
            {
                result.Add(new QueryFilter("StackTrace", QueryFilter.FilterOperator.Contains, criteria.StackTrace));
            }

            return result;
        }

        /// <summary>
        /// To the filters.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;QueryFilter&gt;.</returns>
        public static List<QueryFilter> ToQueryFilters(ApiEventCriteria criteria)
        {
            var result = new List<QueryFilter>();

            if (criteria != null)
            {
                if (criteria.Key != null)
                {
                    result.Add(new QueryFilter("Key", QueryFilter.FilterOperator.Equal, criteria.Key));
                }
                else
                {
                    result.AddRange(ToQueryFilters(criteria as ApiEventLogBase));

                    if (criteria.FromStamp != null)
                    {
                        result.Add(new QueryFilter("CreatedStamp", QueryFilter.FilterOperator.GreaterThanOrEqual, criteria.FromStamp));
                    }

                    if (criteria.ToStamp != null)
                    {
                        result.Add(new QueryFilter("CreatedStamp", QueryFilter.FilterOperator.LessThan, criteria.ToStamp));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// To the query filters.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;QueryFilter&gt;.</returns>
        public static List<QueryFilter> ToQueryFilters(ApiEventLogBase criteria)
        {
            var result = new List<QueryFilter>();

            if (criteria == null) return result;

            criteria.UnifyValue();

            if (!string.IsNullOrWhiteSpace(criteria.ApiFullName))
            {
                result.Add(new QueryFilter("ApiFullName", QueryFilter.FilterOperator.Equal, criteria.ApiFullName));
            }

            if (!string.IsNullOrWhiteSpace(criteria.ResourceAction))
            {
                result.Add(new QueryFilter("ResourceAction", QueryFilter.FilterOperator.Equal, criteria.ResourceAction));
            }

            if (!string.IsNullOrWhiteSpace(criteria.ClientIdentifier))
            {
                result.Add(new QueryFilter("ClientIdentifier", QueryFilter.FilterOperator.Equal, criteria.ClientIdentifier));
            }

            if (criteria.ExceptionKey != null)
            {
                result.Add(new QueryFilter("ExceptionKey", QueryFilter.FilterOperator.Equal, criteria.ExceptionKey));
            }

            if (!string.IsNullOrWhiteSpace(criteria.HttpMethod))
            {
                result.Add(new QueryFilter("HttpMethod", QueryFilter.FilterOperator.Equal, criteria.HttpMethod));
            }

            if (!string.IsNullOrWhiteSpace(criteria.ResourceName))
            {
                result.Add(new QueryFilter("ResourceName", QueryFilter.FilterOperator.Equal, criteria.ResourceName));
            }

            if (!string.IsNullOrWhiteSpace(criteria.ResourceEntityKey))
            {
                result.Add(new QueryFilter("ResourceEntityKey", QueryFilter.FilterOperator.Equal, criteria.ResourceEntityKey));
            }

            if (criteria.ResponseCode != null)
            {
                result.Add(new QueryFilter("ResponseCode", QueryFilter.FilterOperator.Equal, criteria.ResponseCode));
            }

            if (!string.IsNullOrWhiteSpace(criteria.ServerIdentifier))
            {
                result.Add(new QueryFilter("ServerIdentifier", QueryFilter.FilterOperator.Equal, criteria.ServerIdentifier));
            }

            if (!string.IsNullOrWhiteSpace(criteria.ServiceIdentifier))
            {
                result.Add(new QueryFilter("ServiceIdentifier", QueryFilter.FilterOperator.Equal, criteria.ServiceIdentifier));
            }

            if (!string.IsNullOrWhiteSpace(criteria.UserIdentifier))
            {
                result.Add(new QueryFilter("UserIdentifier", QueryFilter.FilterOperator.Equal, criteria.UserIdentifier));
            }

            if (criteria.Platform != null)
            {
                result.Add(new QueryFilter("Platform", QueryFilter.FilterOperator.Equal, (int)criteria.Platform.Value));
            }

            if (criteria.DeviceType != null)
            {
                result.Add(new QueryFilter("DeviceType", QueryFilter.FilterOperator.Equal, (int)criteria.DeviceType.Value));
            }

            if (!string.IsNullOrWhiteSpace(criteria.IpAddress))
            {
                result.Add(new QueryFilter("IpAddress", QueryFilter.FilterOperator.Contains, criteria.IpAddress));
            }

            if (!string.IsNullOrWhiteSpace(criteria.UserAgent))
            {
                result.Add(new QueryFilter("UserAgent", QueryFilter.FilterOperator.Contains, criteria.UserAgent));
            }

            return result;
        }

        /// <summary>
        /// To the group by names.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> ToGroupByNames(ApiEventGroupingCriteria criteria)
        {
            var groupByNames = new List<string>();

            if (criteria != null)
            {
                if (criteria.GroupByResourceName)
                {
                    groupByNames.Add("ResourceName");
                }

                if (criteria.GroupByResourceAction)
                {
                    groupByNames.Add("ResourceAction");
                }

                if (criteria.GroupByHttpMethod)
                {
                    groupByNames.Add("HttpMethod");
                }

                if (criteria.GroupByResponseCode)
                {
                    groupByNames.Add("ResponseCode");
                }

                if (criteria.GroupByServiceIdentifier)
                {
                    groupByNames.Add("ServiceIdentifier");
                }

                if (criteria.GroupByServerIdentifier)
                {
                    groupByNames.Add("ServerIdentifier");
                }

                if (criteria.GroupByLocation)
                {
                    groupByNames.Add("Location.city");
                    groupByNames.Add("Location.country");
                }
            }

            return groupByNames;
        }

        /// <summary>
        /// To the group by names.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> ToGroupByNames(ExceptionGroupingCriteria criteria)
        {
            var groupByNames = new List<string>();

            if (criteria != null)
            {
                if (criteria.GroupByCriticalityLevel)
                {
                    groupByNames.Add("CriticalityLevel");
                }

                if (criteria.GroupByExceptionCode)
                {
                    groupByNames.Add("Code.Major");
                }

                if (criteria.GroupByServiceIdentifier)
                {
                    groupByNames.Add("ServiceIdentifier");
                }

                if (criteria.GroupByServerIdentifier)
                {
                    groupByNames.Add("ServerIdentifier");
                }
            }

            return groupByNames;
        }

        #endregion
    }
}
