using System;
using System.Collections.Generic;
using ifunction.KeenSDK.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ifunction.KeenSDK.UnitTest
{
    /// <summary>
    /// Class KeenClientUnitTest.
    /// </summary>
    [TestClass]
    public class KeenClientUnitTest
    {
        /// <summary>
        /// The client
        /// </summary>
        private static KeenClient client = new KeenClient("TestProjectId",
            "TestMasterKey",
            "TestReadKey",
            "TestWrhiteKey"
            );

        /// <summary>
        /// The collection
        /// </summary>
        const string collection = "Test";

        /// <summary>
        /// Adds the event.
        /// </summary>
        [TestMethod]
        public void AddEvent()
        {
            client.AddEvent(collection, new { Value = Guid.NewGuid() });
        }

        /// <summary>
        /// Deletes the collection.
        /// </summary>
        [TestMethod]
        public void DeleteCollection()
        {
            client.DeleteCollection(collection);
        }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        [TestMethod]
        public void GetSchema()
        {
            var schemaObject = client.GetSchema(collection);

            Assert.IsNotNull(schemaObject);
        }

        /// <summary>
        /// Queries the object.
        /// </summary>
        [TestMethod]
        public void QueryObject()
        {
            var objects = client.QueryObject(collection);

            Assert.IsNotNull(objects);
        }

        /// <summary>
        /// Commons the query group by.
        /// </summary>
        [TestMethod]
        public void CommonQueryGroupBy()
        {
            client.AddEvent(collection, new { Category = "A", Value = Guid.NewGuid() });
            client.AddEvent(collection, new { Category = "B", Value = Guid.NewGuid() });
            client.AddEvent(collection, new { Category = "A", Value = Guid.NewGuid() });

            var result = client.CommonQuery(Model.QueryType.Count, collection, null, null, "Category".CreateList());

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Commons the query multiple group by.
        /// </summary>
        [TestMethod]
        public void CommonQueryMultipleGroupBy()
        {
            client.AddEvent(collection, new { Category = "A", SubCategory = "a", Value = Guid.NewGuid() });
            client.AddEvent(collection, new { Category = "B", SubCategory = "a", Value = Guid.NewGuid() });
            client.AddEvent(collection, new { Category = "A", SubCategory = "b", Value = Guid.NewGuid() });

            var groupBy = new List<string>();
            groupBy.Add("Category");
            groupBy.Add("SubCategory");

            var result = client.CommonQuery(Model.QueryType.Count, collection, null, null, groupBy);

            Assert.IsNotNull(result);
        }
    }
}
