using Moq;

using NUnit.Framework;

using SQLiteKei.Util.Interfaces;
using SQLiteKei.ViewModels.MainWindow;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.ObjectModel;

namespace SQLiteKei.IntegrationTests.ViewModels
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        private MainWindowViewModel viewModel;

        private TableItem LookupItem;

        private IndexFolderItem LookupItemParent;

        private DatabaseItem LookupItemDatabase;

        private TableItem ComparisonItem;

        private IndexFolderItem ComparisonItemParent;

        private Mock<ITreeSaveHelper> treeSaveHelperMock;

        [SetUp]
        public void Setup()
        {
            LookupItem = new TableItem
            {
                DisplayName = "Name",
                DatabasePath = "Database1"
            };

            LookupItemParent = new IndexFolderItem
            {
                DisplayName = "Name",
                DatabasePath = "Database1",
                Items = new ObservableCollection<TreeItem> { LookupItem }
            };

            LookupItemDatabase = new DatabaseItem
            {
                DisplayName = "Name",
                DatabasePath = "Database1",
                Items = new ObservableCollection<TreeItem> { LookupItemParent }
            };

            ComparisonItem = new TableItem
            {
                DisplayName = "Name",
                DatabasePath = "Database2"
            };

            ComparisonItemParent = new IndexFolderItem
            {
                DisplayName = "Name",
                DatabasePath = "Database2",
                Items = new ObservableCollection<TreeItem> { ComparisonItem }
            };

            treeSaveHelperMock = new Mock<ITreeSaveHelper>();

            viewModel = new MainWindowViewModel(treeSaveHelperMock.Object)
            {
                TreeViewItems = new ObservableCollection<TreeItem>
                {
                    LookupItemDatabase,
                    new DatabaseItem
                    {
                        DisplayName = "Name",
                        DatabasePath = "Database2",
                        Items = new ObservableCollection<TreeItem>
                        {
                            ComparisonItemParent
                        }
                    }
                }
            };
        }

        //TODO
        //[Test]
        //public void RemoveItemFromHierarchy_WithExistingItem_RemovesSpecifiedItem()
        //{
        //    viewModel.Delete(LookupItem);

        //    var result = LookupItemParent.Items.Any();
        //    Assert.IsFalse(result);
        //}

        //[Test]
        //public void RemoveItemFromHierarchy_WithExistingItem_DoesNotRemoveItemsWithSameNameFromSameDatabase()
        //{
        //    viewModel.Delete(LookupItem);

        //    var result = LookupItemDatabase.Items.Any();
        //    Assert.IsTrue(result);
        //}

        //[Test]
        //public void RemoveItemFromHierarchy_WithExistingItem_DoesNotRemoveItemsWithSameNameFromOtherDatabase()
        //{
        //    viewModel.Delete(LookupItem);

        //    var result = ComparisonItemParent.Items.Any();
        //    Assert.IsTrue(result);
        //}

        [Test]
        public void SaveTree_CallsSaveTreeHelper()
        {
            viewModel.SaveTree();

            treeSaveHelperMock.Verify(x => x.Save(It.IsAny<ObservableCollection<TreeItem>>()), Times.Once);
        }
    }
}
