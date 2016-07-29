using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class CreateViewQueryBuilderTests
    {
        [Test]
        public void Build_WithValidData_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "CREATE VIEW IF NOT EXISTS ViewName AS\nSELECT * FROM SomeTable";

            var result = QueryBuilder.CreateView("ViewName")
                .IfNotExists(true)
                .As("SELECT * FROM SomeTable")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithoutIfNotExistsMethodCall_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "CREATE VIEW ViewName AS\nSELECT * FROM SomeTable";

            var result = QueryBuilder.CreateView("ViewName")
                .As("SELECT * FROM SomeTable")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
