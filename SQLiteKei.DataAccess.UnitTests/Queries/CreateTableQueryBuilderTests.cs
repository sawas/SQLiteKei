using NUnit.Framework;

using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.DataAccess.QueryBuilders.Enums;

namespace SQLiteKei.UnitTests.Queries
{
    [TestFixture]
    public class CreateTableQueryBuilderTests
    {
        [Test]
        public void Build_WithValidData_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "CREATE TABLE 'Table'\n(\nColumn1 Integer PRIMARY KEY NOT NULL,\nColumn2 Text NOT NULL,\nFOREIGN KEY(Column1) REFERENCES ReferencedTable(ReferencedColumn)\n);";

            var result = QueryBuilder
                .CreateTable("Table")
                .AddColumn("Column1", DataType.Integer, true)
                .AddColumn("Column2", DataType.Text)
                .AddForeignKey("Column1", "ReferencedTable", "ReferencedColumn")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithNulledTableName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.CreateTable(null)
                .AddColumn("Column", DataType.Bool)
                .Build());
        }

        [Test]
        public void Build_WithEmptyTableName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.CreateTable(string.Empty)
                .AddColumn("Column", DataType.Bool)
                .Build());
        }

        [Test]
        public void Build_WithWhitespacedTableName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.CreateTable("  ")
                .AddColumn("Column", DataType.Bool)
                .Build());
        }

        [Test]
        public void Build_WithoutColumn_ThrowsException()
        {
            Assert.Throws(typeof(ColumnDefinitionException),
                () => QueryBuilder.CreateTable("Table")
                .Build());
        }

        [Test]
        public void Build_WithColumnsWithTheSameName_ThrowsException()
        {
            Assert.Throws(typeof(ColumnDefinitionException),
                () => QueryBuilder.CreateTable("Table")
                .AddColumn("Column", DataType.Bool)
                .AddColumn("Column", DataType.Blob)
                .Build());
        }

        [Test]
        public void Build_WithMultiplePrimaryKeys_ThrowsException()
        {
            Assert.Throws(typeof(ColumnDefinitionException),
                () => QueryBuilder.CreateTable("Table")
                .AddColumn("Column", DataType.Bool, true)
                .AddColumn("Column1", DataType.Blob, true)
                .Build());
        }

        [Test]
        public void AddColumn_WithEmptyColumnName_ThrowsException()
        {
            Assert.Throws(typeof(ColumnDefinitionException),
                () => QueryBuilder.CreateTable("Table")
                .AddColumn(null, DataType.Bool));
        }

        [Test]
        public void AddColumn_WithNulledColumnName_ThrowsException()
        {
            Assert.Throws(typeof(ColumnDefinitionException),
                () => QueryBuilder.CreateTable("Table")
                .AddColumn(string.Empty, DataType.Bool));
        }

        [Test]
        public void AddColumn_WithWhitespacedColumnName_ThrowsException()
        {
            Assert.Throws(typeof(ColumnDefinitionException),
                () => QueryBuilder.CreateTable("Table")
                .AddColumn("  ", DataType.Bool));
        }

        [Test]
        public void AddForeignKey_WithKeysWithTheSameName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.CreateTable("Table")
                .AddForeignKey("Local", "RefTable", "RefCol")
                .AddForeignKey("Local", "RefTable", "RefCol"));
        }
    }
}
