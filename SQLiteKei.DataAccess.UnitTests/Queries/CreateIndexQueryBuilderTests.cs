using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.DataAccess.QueryBuilders.Enums;

namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class CreateIndexQueryBuilderTests
    {
        [Test]
        public void Build_WithValidValues_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "CREATE UNIQUE INDEX IF NOT EXISTS 'IndexName'\nON 'TableName' ('Column1' ASC, 'Column2' DESC)\nWHERE Condition";

            var result = QueryBuilder.CreateIndex("IndexName")
                .Unique()
                .IfNotExists()
                .On("TableName")
                .IndexColumn("Column1", OrderType.Ascending)
                .IndexColumn("Column2", OrderType.Descending)
                .Where("Condition")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
