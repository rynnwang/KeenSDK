using System;
using ifunction.RestApi;
using Newtonsoft.Json.Linq;

namespace ifunction.KeenSDK.Model
{
    /// <summary>
    /// Class QueryTimeFrame.
    /// </summary>
    public class QueryTimeFrame
    {
        #region Enum

        /// <summary>
        /// Enum FrameType
        /// </summary>
        public enum FrameType
        {
            /// <summary>
            /// The this
            /// </summary>
            This = 0,
            /// <summary>
            /// The previous
            /// </summary>
            Previous = 1
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is absolute.
        /// </summary>
        /// <value><c>true</c> if this instance is absolute; otherwise, <c>false</c>.</value>
        public bool IsAbsolute { get; protected set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public FrameType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the n.
        /// </summary>
        /// <value>The n.</value>
        public int N { get; protected set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>The unit.</value>
        public TimeUnit Unit { get; protected set; }

        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        public DateTime? FromStamp { get; protected set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        public DateTime? ToStamp { get; protected set; }

        #endregion

        #region Override

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (this.IsAbsolute)
            {
                return JObject.FromObject(new { start = this.FromStamp, end = this.ToStamp }).ToString();
            }
            else
            {
                return string.Format(this.N > 0 ? "{0}_{2}_{1}" : "{0}_{1}",
                    this.Type.ToString().ToLowerInvariant(),
                    this.Unit.ToString().ToLowerInvariant(),
                    this.N > 0 ? this.N : 1
                    );
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToSafeString());
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTimeFrame"/> class.
        /// </summary>
        /// <param name="fromStamp">From stamp.</param>
        /// <param name="toStamp">To stamp.</param>
        protected QueryTimeFrame(DateTime? fromStamp, DateTime? toStamp)
        {
            this.IsAbsolute = true;
            this.FromStamp = fromStamp;
            this.ToStamp = toStamp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTimeFrame"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="n">The n.</param>
        /// <param name="timeUnit">The time unit.</param>
        protected QueryTimeFrame(FrameType type, int n, TimeUnit timeUnit)
        {
            this.IsAbsolute = false;
            this.Type = type;
            this.Unit = timeUnit;
        }

        #endregion

        #region Static generation (absolute)

        /// <summary>
        /// Absolutes the time frame.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>QueryTimeFrame.</returns>
        /// <exception cref="ArgumentException">Start date must be before end date.</exception>
        public static QueryTimeFrame AbsoluteTimeFrame(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ArgumentException("Start date must be before end date.");
            }

            return new QueryTimeFrame(start, end);
        }

        #endregion

        #region Static generation (relative)

        /// <summary>
        /// Creates a TimeFrame starting from the beginning of the current minute until now.
        /// </summary>
        public static QueryTimeFrame ThisMinute()
        {
            return new QueryTimeFrame(FrameType.This, 1, TimeUnit.Minute);
        }

        /// <summary>
        /// Creates a TimeFrame starting from the beginning of the current hour until now.
        /// </summary>
        public static QueryTimeFrame ThisHour()
        {
            return new QueryTimeFrame(FrameType.This, 1, TimeUnit.Hour);
        }

        /// <summary>
        /// Creates a TimeFrame starting from the beginning of the current day until now.
        /// </summary>
        public static QueryTimeFrame ThisDay()
        {
            return new QueryTimeFrame(FrameType.This, 1, TimeUnit.Day);
        }

        /// <summary>
        /// Creates a TimeFrame starting from the beginning of the current week until now.
        /// </summary>
        public static QueryTimeFrame ThisWeek()
        {
            return new QueryTimeFrame(FrameType.This, 1, TimeUnit.Week);
        }

        /// <summary>
        /// Creates a TimeFrame starting from the beginning of the current month until now.
        /// </summary>
        public static QueryTimeFrame ThisMonth()
        {
            return new QueryTimeFrame(FrameType.This, 1, TimeUnit.Month);
        }

        /// <summary>
        /// Creates a TimeFrame starting from the beginning of the current year until now.
        /// </summary>
        public static QueryTimeFrame ThisYear()
        {
            return new QueryTimeFrame(FrameType.This, 1, TimeUnit.Year);
        }

        /// <summary>
        /// All of the current minute and the previous completed n-1 minutes.
        /// </summary>
        public static QueryTimeFrame ThisNMinutes(int n)
        {
            return new QueryTimeFrame(FrameType.This, n, TimeUnit.Minute);
        }

        /// <summary>
        /// All of the current hour and the previous completed n-1 hours.
        /// </summary>
        public static QueryTimeFrame ThisNHours(int n)
        {
            return new QueryTimeFrame(FrameType.This, n, TimeUnit.Hour);
        }

        /// <summary>
        /// All of the current day and the previous completed n-1 days.
        /// </summary>
        public static QueryTimeFrame ThisNDays(int n)
        {
            return new QueryTimeFrame(FrameType.This, n, TimeUnit.Day);
        }

        /// <summary>
        /// All of the current week and the previous completed n-1 weeks.
        /// </summary>
        public static QueryTimeFrame ThisNWeeks(int n)
        {
            return new QueryTimeFrame(FrameType.This, n, TimeUnit.Week);
        }

        /// <summary>
        /// All the current month and previous completed n-1 months.
        /// </summary>
        public static QueryTimeFrame ThisNMonths(int n)
        {
            return new QueryTimeFrame(FrameType.This, n, TimeUnit.Month);
        }

        /// <summary>
        /// All the current year and previous completed n-1 years.
        /// </summary>
        public static QueryTimeFrame ThisNYears(int n)
        {
            return new QueryTimeFrame(FrameType.This, n, TimeUnit.Year);
        }

        /// <summary>
        /// Gives a start of n-minutes before the most recent complete minute and an end at the most recent complete minute. 
        /// <para>Example: If right now it is 7:15:30pm and I specify “previous_3_minutes”, the TimeFrame would stretch from 7:12pm until 7:15pm.</para>
        /// </summary>
        public static QueryTimeFrame PreviousNMinutes(int n)
        {
            return new QueryTimeFrame(FrameType.Previous, n, TimeUnit.Minute);
        }

        /// <summary>
        /// Gives a start of n-hours before the most recent complete hour and an end at the most recent complete hour. 
        /// <para>Example: If right now it is 7:15pm and I specify “previous_7_hours”, the TimeFrame would stretch from noon until 7:00pm.</para>
        /// </summary>
        public static QueryTimeFrame PreviousNHours(int n)
        {
            return new QueryTimeFrame(FrameType.Previous, n, TimeUnit.Hour);
        }

        /// <summary>
        /// Gives a starting point of n-days before the most recent complete day and an end at the most recent complete day. 
        /// <para>Example: If right now is Friday at 9:00am and I specify a TimeFrame of “previous_3_days”, the TimeFrame would stretch from Tuesday morning at 12:00am until Thursday night at midnight.</para>
        /// </summary>
        public static QueryTimeFrame PreviousNDays(int n)
        {
            return new QueryTimeFrame(FrameType.Previous, n, TimeUnit.Day);
        }

        /// <summary>
        /// Gives a start of n-weeks before the most recent complete week and an end at the most recent complete week. 
        /// <para>Example: If right now is Monday and I specify a TimeFrame of “previous_2_weeks”, the TimeFrame would stretch from three Sunday mornings ago at 12:00am until the most recent Sunday at 12:00am. (yesterday morning)</para>
        /// </summary>
        public static QueryTimeFrame PreviousNWeeks(int n)
        {
            return new QueryTimeFrame(FrameType.Previous, n, TimeUnit.Week);
        }

        /// <summary>
        /// Gives a start of n-months before the most recent completed month and an end at the most recent completed month. 
        /// <para>Example: If right now is the 5th of the month and I specify a TimeFrame of “previous_2_months”, the TimeFrame would stretch from the start of two months ago until the end of last month.</para>
        /// </summary>
        public static QueryTimeFrame PreviousNMonths(int n)
        {
            return new QueryTimeFrame(FrameType.Previous, n, TimeUnit.Month);
        }

        /// <summary>
        /// Gives a start of n-years before the most recent completed year and an end at the most recent completed year. 
        /// <para>Example: If right now is the June 5th and I specify a TimeFrame of “previous_2_years”, the TimeFrame would stretch from the start of two years ago until the end of last year.</para>
        /// </summary>
        public static QueryTimeFrame PreviousNYears(int n)
        {
            return new QueryTimeFrame(FrameType.Previous, n, TimeUnit.Year);
        }

        /// <summary>
        /// convenience for “previous_1_minute”
        /// </summary>
        public static QueryTimeFrame PreviousMinute()
        {
            return new QueryTimeFrame(FrameType.Previous, 1, TimeUnit.Minute);
        }

        /// <summary>
        /// convenience for “previous_1_hour”
        /// </summary>
        public static QueryTimeFrame PreviousHour()
        {
            return new QueryTimeFrame(FrameType.Previous, 1, TimeUnit.Hour);
        }

        /// <summary>
        ///  previous_day - convenience for “previous_1_day”
        /// </summary>
        public static QueryTimeFrame Yesterday()
        {
            return new QueryTimeFrame(FrameType.Previous, 1, TimeUnit.Day);
        }

        /// <summary>
        /// convenience for “previous_1_week”
        /// </summary>
        public static QueryTimeFrame PreviousWeek()
        {
            return new QueryTimeFrame(FrameType.Previous, 1, TimeUnit.Week);
        }

        /// <summary>
        /// convenience for “previous_1_months”
        /// </summary>
        public static QueryTimeFrame PreviousMonth()
        {
            return new QueryTimeFrame(FrameType.Previous, 1, TimeUnit.Month);
        }

        /// <summary>
        /// convenience for “previous_1_years”
        /// </summary>
        public static QueryTimeFrame PreviousYear()
        {
            return new QueryTimeFrame(FrameType.Previous, 1, TimeUnit.Year);
        }

        #endregion
    }
}
