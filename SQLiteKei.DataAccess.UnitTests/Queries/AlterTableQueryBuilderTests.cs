using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class AlterTableQueryBuilderTests
    {
        [Test]
        public void Build_WithValidValues_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "ALTER TABLE 'TableName'\nADD COLUMN ColumnName DataType NOT NULL DEFAULT 'Default';";

            var result = QueryBuilder.AlterTable("TableName")
                .AddColumn("ColumnName", "DataType", true, "Default")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
