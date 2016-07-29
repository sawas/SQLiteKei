using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class DropViewQueryBuilderTests
    {
        [Test]
        public void Build_WithValidData_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP VIEW IF EXISTS ViewName";

            var result = QueryBuilder.DropView("ViewName").IfExists().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithoutIfExistsMethodCall_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP VIEW ViewName";

            var result = QueryBuilder.DropView("ViewName").Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
