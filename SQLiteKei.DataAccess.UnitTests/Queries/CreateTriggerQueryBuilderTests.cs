using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class CreateTriggerQueryBuilderTests
    {
        [Test]
        public void Build_WithValidValues_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "CREATE TRIGGER IF NOT EXISTS 'TriggerName'\nAFTER UPDATE ON 'TargetTable'\nFOR EACH ROW\nWHEN Condition\nBEGIN\nSQL Statement;\nEND";

            var result = QueryBuilder.CreateTrigger("TriggerName")
                .IfNotExists()
                .After().Update().On("TargetTable")
                .ForEachRow()
                .When("Condition")
                .Do("SQL Statement")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
