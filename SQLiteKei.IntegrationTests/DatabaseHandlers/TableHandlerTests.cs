using NUnit.Framework;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.IntegrationTests.Base;

using System;
using System.Data;
using System.Linq;

namespace SQLiteKei.IntegrationTests.DatabaseHandlers
{
    [TestFixture, Explicit]
    public class TableHandlerTests : IntegrationTestBase
    {
        private TableHandler tableHandler;

        [SetUp]
        public void SetUp()
        {
            // Close the auto-generated connection and replace it with the one used in the IntegrationTestBase
            tableHandler = new TableHandler(targetDatabaseFilePath);
            tableHandler.connection.Close();
            tableHandler.connection = connection;
            tableHandler.connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            tableHandler.connection.Close();
        }

        [Test]
        public void GetColumns_WithValidTableName_ReturnsCorrectNumberOfColumns()
        {
            var result = tableHandler.GetColumns("TEST5");
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void GetCreateStatement_ValidTableName_ReturnsValidCreateStatement()
        {
            var result = tableHandler.GetCreateStatement("TEST10");

            Assert.AreEqual("CREATE TABLE TEST10 (ColumnA10 varchar(60), ColumnB10 int)", result);
        }

        [Test]
        public void GetCreateStatement_WithInvalidTableName_ThrowsException()
        {
            Assert.Throws(typeof(TableNotFoundException),
                () => tableHandler.GetCreateStatement("TABLE_INVALID"));
        }

        [Test]
        public void EmptyTable_WithValidTableName_EmptiesTable()
        {
            tableHandler.EmptyTable("TEST10");

            var command = connection.CreateCommand();
            command.CommandText = "SELECT count(*) FROM TEST10";
            var result = Convert.ToInt64(command.ExecuteScalar());

            Assert.AreEqual(0, result);
        }

        [Test]
        public void EmptyTable_WithInvalidTableName_ThrowsException()
        {
            Assert.Throws(typeof(TableNotFoundException),
                () => tableHandler.EmptyTable("TABLE_INVALID"));
        }

        [Test]
        public void DropTable_WithValidTableName_DropsTable()
        {
            tableHandler.DropTable("TEST1");

            var tables = connection.GetSchema("Tables").AsEnumerable();
            var result = tables.SingleOrDefault(x => x.ItemArray[1].Equals("TEST1"));

            Assert.IsNull(result);
        }
    }
}
