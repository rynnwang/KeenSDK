﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ifunction.KeenSDK.Model
{
    /// <summary>
    /// Represents the values from a query performed with a groupby. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class QueryGroupValue<T>
    {
        /// <summary>
        /// The value for the group. Varies with the type of query performed.
        /// </summary>
        public T Value { get; private set; }
        
        /// <summary>
        /// The value of the groupby field for this value.
        /// </summary>
        public string Group { get; private set; }

        public QueryGroupValue(T value, string group)
        {
            Value = value;
            Group = group;
        }
    }
}
