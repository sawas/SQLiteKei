using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class UpdateQueryBuilderTests
    {
        [Test]
        public void Build_WithValidValues_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "UPDATE 'TableName'\nSET ColumnA='B', ColumnB='A'\nWHERE ColumnA = 'A'\nAND ColumnB = 'B'";

            var result = QueryBuilder.Update("TableName")
                .Set("ColumnA", "B")
                .Set("ColumnB", "A")
                .Where("ColumnA").Is("A")
                .And("ColumnB").Is("B")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
