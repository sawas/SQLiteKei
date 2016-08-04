using NUnit.Framework;

using SQLiteKei.IntegrationTests.Base;
using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.TriggerCreatorWindow;

namespace SQLiteKei.IntegrationTests.ViewModels
{
    [TestFixture, Explicit]
    public class TriggerCreatorViewModelTests : IntegrationTestBase
    {
        private TriggerCreatorViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            viewModel = new TriggerCreatorViewModel();
        }

        [Test]
        public void IsValidModelProperty_WithValidValues_IsTrue()
        {
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = targetDatabaseFilePath };
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
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = targetDatabaseFilePath };
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
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = targetDatabaseFilePath };
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
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = targetDatabaseFilePath };
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
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = targetDatabaseFilePath };
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
            viewModel.SelectedDatabase = new DatabaseSelectItem { DatabaseName = "DB", DatabasePath = targetDatabaseFilePath };
            viewModel.SelectedTarget = "Table";
            viewModel.SelectedTriggerEntryPoint = "EntryPoint";
            viewModel.SelectedTriggerEvent = "Event";
            viewModel.TriggerActions = "Actions";
            viewModel.TriggerName = string.Empty;

            Assert.IsFalse(viewModel.IsValidModel);
        }
    }
}
