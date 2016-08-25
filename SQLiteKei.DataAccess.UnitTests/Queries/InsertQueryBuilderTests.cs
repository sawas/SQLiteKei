using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

using System.Collections.Generic;


namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class InsertQueryBuilderTests
    {
        [Test]
        public void Build_WithValidValues_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "INSERT INTO 'TableName' VALUES (\"1\",\"3\",\"abc\");";

            var values = new List<string>
            {
                "1", "3", "abc"
            };

            var result = QueryBuilder.InsertInto("TableName")
                .Values(values)
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
