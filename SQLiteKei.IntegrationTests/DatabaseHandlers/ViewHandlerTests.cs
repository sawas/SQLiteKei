using NUnit.Framework;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.IntegrationTests.Base;

using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace SQLiteKei.IntegrationTests.DatabaseHandlers
{
    [TestFixture]
    public class ViewHandlerTests : IntegrationTestBase
    {
        private ViewHandler viewHandler;

        [SetUp]
        public void SetUp()
        {
            viewHandler = new ViewHandler(connection);
        }

        [TearDown]
        public void TearDown()
        {
            viewHandler.Dispose();
        }

        [Test]
        public void DropView_DropsViewFromDatabase()
        {
            viewHandler.Drop("View1");

            var result = FindView("View1");

            Assert.IsNull(result);
        }

        [Test]
        public void UpdateViewName_WithValidValues_UpdatesViewName()
        {
            viewHandler.UpdateViewName("View1", "NewName");

            var result = FindView("NewName");

            Assert.IsNotNull(result);
        }

        [Test]
        public void UpdateViewName_WithAlreadyExistingTargetView_ThrowsException()
        {
            Assert.Throws(typeof(SQLiteException),
                () => viewHandler.UpdateViewName("View1", "View2"));
        }

        [Test]
        public void UpdateViewDefinition_WithValidValues_UpdatesViewDefinition()
        {
            viewHandler.UpdateViewDefinition("View1", "SELECT * FROM TEST2");

            var view = FindView("View1");
            var result = view.ItemArray[3];

            Assert.AreEqual("SELECT * FROM TEST2", result);
        }

        [Test]
        public void UpdateViewDefinition_WithInvalidStatement_ThrowsException()
        {
            Assert.Throws(typeof(SQLiteException),
                () => viewHandler.UpdateViewDefinition("View1", "InvalidStatement"));
        }

        private DataRow FindView(string viewName)
        {
            var dataRows = connection.GetSchema("Views").AsEnumerable();
            return dataRows.SingleOrDefault(x => x.ItemArray[2].Equals(viewName));
        }
    }
}
