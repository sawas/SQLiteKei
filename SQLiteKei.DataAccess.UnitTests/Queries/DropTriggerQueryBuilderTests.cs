using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class DropTriggerQueryBuilderTests
    {
        [Test]
        public void Build_WithValidData_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TRIGGER IF EXISTS TriggerName";

            var result = QueryBuilder.DropTrigger("TriggerName").IfExists().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithoutIfExistsMethodCall_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TRIGGER TriggerName";

            var result = QueryBuilder.DropTrigger("TriggerName").Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
