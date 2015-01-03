using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ifunction.KeenSDK.Model
{
    public class QueryInterval
    {
        #region Enum

        /// <summary>
        /// Enum IntervalType
        /// </summary>
        public enum IntervalType
        {
            /// <summary>
            /// The none
            /// </summary>
            None = 0,
            /// <summary>
            /// The minutely
            /// </summary>
            Minutely = 1,
            /// <summary>
            /// The hourly
            /// </summary>
            Hourly = 2,
            /// <summary>
            /// The daily
            /// </summary>
            Daily = 3,
            /// <summary>
            /// The weekly
            /// </summary>
            Weekly = 4,
            /// <summary>
            /// The monthly
            /// </summary>
            Monthly = 5,
            /// <summary>
            /// The yearly
            /// </summary>
            Yearly = 6,
            /// <summary>
            /// The every n times
            /// </summary>
            EveryNTimes = 7
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public IntervalType Type
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the n.
        /// </summary>
        /// <value>The n.</value>
        public int N
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the n unit.
        /// </summary>
        /// <value>The n unit.</value>
        public TimeUnit Unit
        {
            get;
            protected set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryInterval"/> class.
        /// </summary>
        protected QueryInterval()
        {

        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            switch (this.Type)
            {
                case IntervalType.None:
                    return string.Empty;
                case IntervalType.EveryNTimes:
                    return this.N > 0 ? string.Format("every_{0}_{1}", this.N, this.Unit.ToString().ToLowerInvariant()) : string.Empty;
                default:
                    return this.Type.ToString().ToLowerInvariant();
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

        /// <summary>
        /// breaks your TimeFrame into minute long chunks.
        /// </summary>
        /// <value>The minutely.</value>
        public static QueryInterval Minutely
        {
            get
            {
                return new QueryInterval
                {
                    N = 1,
                    Type = IntervalType.Minutely,
                    Unit = TimeUnit.Minute
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into hour long chunks.
        /// </summary>
        /// <value>The hourly.</value>
        public static QueryInterval Hourly
        {
            get
            {
                return new QueryInterval
                {
                    N = 1,
                    Type = IntervalType.Hourly,
                    Unit = TimeUnit.Hour
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into day long chunks.
        /// </summary>
        /// <value>The daily.</value>
        public static QueryInterval Daily
        {
            get
            {
                return new QueryInterval
                {
                    N = 1,
                    Type = IntervalType.Daily,
                    Unit = TimeUnit.Day
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into week long chunks.
        /// </summary>
        /// <value>The weekly.</value>
        public static QueryInterval Weekly
        {
            get
            {
                return new QueryInterval
                {
                    N = 1,
                    Type = IntervalType.Weekly,
                    Unit = TimeUnit.Week
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into month long chunks.
        /// </summary>
        /// <value>The monthly.</value>
        public static QueryInterval Monthly
        {
            get
            {
                return new QueryInterval
                {
                    N = 1,
                    Type = IntervalType.Monthly,
                    Unit = TimeUnit.Month
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into year long chunks.
        /// </summary>
        /// <value>The yearly.</value>
        public static QueryInterval Yearly
        {
            get
            {
                return new QueryInterval
                {
                    N = 1,
                    Type = IntervalType.Yearly,
                    Unit = TimeUnit.Year
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>QueryInterval.</returns>
        public static QueryInterval EveryNMinutes(int n)
        {
            return new QueryInterval
            {
                N = n,
                Type = IntervalType.EveryNTimes,
                Unit = TimeUnit.Minute
            };
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>QueryInterval.</returns>
        public static QueryInterval EveryNHours(int n)
        {
            return new QueryInterval
            {
                N = n,
                Type = IntervalType.EveryNTimes,
                Unit = TimeUnit.Hour
            };
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>QueryInterval.</returns>
        public static QueryInterval EveryNDays(int n)
        {
            return new QueryInterval
            {
                N = n,
                Type = IntervalType.EveryNTimes,
                Unit = TimeUnit.Day
            };
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>QueryInterval.</returns>
        public static QueryInterval EveryNWeeks(int n)
        {
            return new QueryInterval
            {
                N = n,
                Type = IntervalType.EveryNTimes,
                Unit = TimeUnit.Week
            };
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>QueryInterval.</returns>
        public static QueryInterval EveryNMonths(int n)
        {
            return new QueryInterval
            {
                N = n,
                Type = IntervalType.EveryNTimes,
                Unit = TimeUnit.Month
            };
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>QueryInterval.</returns>
        public static QueryInterval EveryNYears(int n)
        {
            return new QueryInterval
            {
                N = n,
                Type = IntervalType.EveryNTimes,
                Unit = TimeUnit.Year
            };
        }
    }
}
