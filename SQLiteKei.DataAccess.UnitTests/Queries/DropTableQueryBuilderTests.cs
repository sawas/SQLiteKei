#region usings

using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

#endregion

namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class DropTableQueryBuilderTests
    {
        [Test]
        public void Build_SimpleDrop_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE 'Table'";
            var result = QueryBuilder.DropTable("Table").Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithIfExistsCase_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE IF EXISTS 'Table'";

            var result = QueryBuilder.DropTable("Table").IfExists().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithCascadeCase_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE 'Table' CASCADE";
            var result = QueryBuilder.DropTable("Table").Cascade().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithAllCases_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE IF EXISTS 'Table' CASCADE";
            var result = QueryBuilder.DropTable("Table").IfExists().Cascade().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithMultipleIfExistsCases_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE IF EXISTS 'Table'";

            var result = QueryBuilder.DropTable("Table").IfExists().IfExists().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithMultipleCascadeCases_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE 'Table' CASCADE";

            var result = QueryBuilder.DropTable("Table").Cascade().Cascade().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
