using NUnit.Framework;

using SQLiteKei.IntegrationTests.Base;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.CreatorWindows.TriggerCreatorWindow;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.ObjectModel;

namespace SQLiteKei.IntegrationTests.ViewModels
{
    [TestFixture]
    public class TriggerCreatorViewModelTests : IntegrationTestBase
    {
        private TriggerCreatorViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            MainTreeHandler.Register(new ObservableCollection<TreeItem>());
            viewModel = new TriggerCreatorViewModel();
        }

        [Test]
        public void IsValidModelProperty_WithValidValues_IsTrue()
        {
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = testDatabaseFile };
            viewModel.SelectedTarget = "Table";
            viewModel.SelectedTriggerEntryPoint = "EntryPoint";
            viewModel.SelectedTriggerEvent = "Event";
            viewModel.TriggerActions = "Actions";
            viewModel.TriggerName = "Trigger";

            Assert.IsTrue(viewModel.IsValidModel);
        }

        [Test]
        public void IsValidModelProperty_WithoutSelectedTable_IsFalse()
        {
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = testDatabaseFile };
            viewModel.SelectedTarget = string.Empty;
            viewModel.SelectedTriggerEntryPoint = "EntryPoint";
            viewModel.SelectedTriggerEvent = "Event";
            viewModel.TriggerActions = "Actions";
            viewModel.TriggerName = "Trigger";

            Assert.IsFalse(viewModel.IsValidModel);
        }

        [Test]
        public void IsValidModelProperty_WithoutSelectedEntryPoint_IsFalse()
        {
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = testDatabaseFile };
            viewModel.SelectedTarget = "Table";
            viewModel.SelectedTriggerEntryPoint = string.Empty;
            viewModel.SelectedTriggerEvent = "Event";
            viewModel.TriggerActions = "Actions";
            viewModel.TriggerName = "Trigger";

            Assert.IsFalse(viewModel.IsValidModel);
        }

        [Test]
        public void IsValidModelProperty_WithoutSelectedTriggerEvent_IsFalse()
        {
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = testDatabaseFile };
            viewModel.SelectedTarget = "Table";
            viewModel.SelectedTriggerEntryPoint = "EntryPoint";
            viewModel.SelectedTriggerEvent = string.Empty;
            viewModel.TriggerActions = "Actions";
            viewModel.TriggerName = "Trigger";

            Assert.IsFalse(viewModel.IsValidModel);
        }

        [Test]
        public void IsValidModelProperty_WithoutTriggerActions_IsFalse()
        {
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = testDatabaseFile };
            viewModel.SelectedTarget = "Table";
            viewModel.SelectedTriggerEntryPoint = "EntryPoint";
            viewModel.SelectedTriggerEvent = "Event";
            viewModel.TriggerActions = string.Empty;
            viewModel.TriggerName = "Trigger";

            Assert.IsFalse(viewModel.IsValidModel);
        }

        [Test]
        public void IsValidModelProperty_WithoutTriggerName_IsFalse()
        {
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = testDatabaseFile };
            viewModel.SelectedTarget = "Table";
            viewModel.SelectedTriggerEntryPoint = "EntryPoint";
            viewModel.SelectedTriggerEvent = "Event";
            viewModel.TriggerActions = "Actions";
            viewModel.TriggerName = string.Empty;

            Assert.IsFalse(viewModel.IsValidModel);
        }
    }
}
