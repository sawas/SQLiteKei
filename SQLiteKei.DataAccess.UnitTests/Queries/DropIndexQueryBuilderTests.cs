using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class DropIndexQueryBuilderTests
    {
        [Test]
        public void Build_WithValidData_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP INDEX IF EXISTS 'IndexName'";

            var result = QueryBuilder.DropIndex("IndexName").IfExists().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithoutIfExistsMethodCall_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP INDEX 'IndexName'";

            var result = QueryBuilder.DropIndex("IndexName").Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
