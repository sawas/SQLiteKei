using NUnit.Framework;
using SQLiteKei.UnitTests.TestUtils;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.ObjectModel;

namespace SQLiteKei.UnitTests.Util
{
    [TestFixture]
    public class MainTreeHandlerTests
    {
        private ObservableCollection<TreeItem> testTree;

        private const string DATABASEPATH1 = "TestPath1";

        private const string DATABASEPATH2 = "TestPath2";

        [SetUp]
        public void SetUp()
        {
            testTree = new ObservableCollection<TreeItem>
            {
                new DatabaseItem
                {
                    DisplayName = "Db1",
                    DatabasePath = DATABASEPATH1,
                    Items = new ObservableCollection<TreeItem>
                    {
                        new TableFolderItem
                        {
                            DatabasePath = DATABASEPATH1,
                            Items = new ObservableCollection<TreeItem>
                            {
                                new TableItem
                                {
                                    DisplayName = "Table",
                                    DatabasePath = DATABASEPATH1
                                }
                            }
                        },
                        new ViewFolderItem
                        {
                            DatabasePath = DATABASEPATH1,
                            Items = new ObservableCollection<TreeItem>
                            {
                                new ViewItem
                                {
                                    DisplayName = "View",
                                    DatabasePath = DATABASEPATH1
                                }
                            }
                        },
                        new TriggerFolderItem
                        {
                            DatabasePath = DATABASEPATH1,
                            Items = new ObservableCollection<TreeItem>
                            {
                                new TriggerItem
                                {
                                    DisplayName = "Trigger",
                                    DatabasePath = DATABASEPATH1
                                }
                            }
                        }
                    }
                },
                new DatabaseItem
                {
                    DisplayName = "Db2",
                    DatabasePath = DATABASEPATH2,
                    Items = new ObservableCollection<TreeItem>
                    {
                        new TableFolderItem
                        {
                            DatabasePath = DATABASEPATH2,
                            Items = new ObservableCollection<TreeItem>
                            {
                                new TableItem
                                {
                                    DisplayName = "Table",
                                    DatabasePath = DATABASEPATH2
                                }
                            }
                        },
                        new ViewFolderItem
                        {
                            DatabasePath = DATABASEPATH2,
                            Items = new ObservableCollection<TreeItem>
                            {
                                new ViewItem
                                {
                                    DisplayName = "View",
                                    DatabasePath = DATABASEPATH2
                                }
                            }
                        },
                        new TriggerFolderItem
                        {
                            DatabasePath = DATABASEPATH1,
                            Items = new ObservableCollection<TreeItem>
                            {
                                new TriggerItem
                                {
                                    DisplayName = "Trigger",
                                    DatabasePath = DATABASEPATH2
                                }
                            }
                        }
                    }
                }
            };

            MainTreeHandler.Register(testTree);
        }

        [Test]
        public void AddTable_WithValidDatabasePath_AddsTableToSpecifiedDatabase()
        {
            MainTreeHandler.AddTable("NewTable", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<TableFolderItem>(testTree,"NewTable", DATABASEPATH1);
            Assert.IsTrue(result);
        }

        [Test]
        public void AddTable_WithValidDatabasePath_DoesNotAddTableToDifferentDatabase()
        {
            MainTreeHandler.AddTable("NewTable", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<TableFolderItem>(testTree, "NewTable", DATABASEPATH2);
            Assert.IsFalse(result);
        }

        [Test]
        public void AddView_WithValidDatabasePath_AddsViewToSpecifiedDatabase()
        {
            MainTreeHandler.AddView("NewView", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<ViewFolderItem>(testTree, "NewView", DATABASEPATH1);
            Assert.IsTrue(result);
        }

        [Test]
        public void AddView_WithValidDatabasePath_DoesNotAddViewToDifferentDatabase()
        {
            MainTreeHandler.AddView("NewView", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<ViewFolderItem>(testTree, "NewView", DATABASEPATH2);
            Assert.IsFalse(result);
        }

        [Test]
        public void AddTrigger_WithValidDatabasePath_AddsTriggerToSpecifiedDatabase()
        {
            MainTreeHandler.AddTrigger("NewTrigger", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<TriggerFolderItem>(testTree, "NewTrigger", DATABASEPATH1);
            Assert.IsTrue(result);
        }

        [Test]
        public void AddTrigger_WithValidDatabasePath_DoesNotAddTriggerToDifferentDatabase()
        {
            MainTreeHandler.AddTrigger("NewTrigger", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<TriggerFolderItem>(testTree, "NewTrigger", DATABASEPATH2);
            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateTableName_WithValidDatabasePath_RenamesTableInSpecifiedDatabase()
        {
            MainTreeHandler.UpdateTableName("Table", "NewTable", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<TableFolderItem>(testTree, "NewTable", DATABASEPATH1);
            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateTableName_WithValidDatabasePath_DatabaseDoesNotContainOldItemAnymore()
        {
            MainTreeHandler.UpdateTableName("Table", "NewTable", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<TableFolderItem>(testTree, "Table", DATABASEPATH1);
            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateViewName_WithValidDatabasePath_RenamesViewInSpecifiedDatabase()
        {
            MainTreeHandler.UpdateViewName("View", "NewView", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<ViewFolderItem>(testTree, "NewView", DATABASEPATH1);
            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateViewName_WithValidDatabasePath_DatabaseDoesNotContainOldItemAnymore()
        {
            MainTreeHandler.UpdateViewName("View", "NewView", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<ViewFolderItem>(testTree, "View", DATABASEPATH1);
            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateTriggerName_WithValidDatabasePath_RenamesTriggerInSpecifiedDatabase()
        {
            MainTreeHandler.UpdateTriggerName("Trigger", "NewTrigger", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<TriggerFolderItem>(testTree, "NewTrigger", DATABASEPATH1);
            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateTriggerName_WithValidDatabasePath_DatabaseDoesNotContainOldItemAnymore()
        {
            MainTreeHandler.UpdateTriggerName("Trigger", "NewTrigger", DATABASEPATH1);

            var result = TreeSearcher.DatabaseHoldsItem<TriggerFolderItem>(testTree, "Trigger", DATABASEPATH1);
            Assert.IsFalse(result);
        }

        [Test]
        public void AddTable_WithInvalidDatabasePath_DoesNotThrowException()
        {
            Assert.DoesNotThrow(
                () => MainTreeHandler.AddTable("NewTable", "InvalidPath"));            
        }

        [Test]
        public void AddView_WithInvalidDatabasePath_DoesNotThrowException()
        {
            Assert.DoesNotThrow(
                () => MainTreeHandler.AddView("NewView", "InvalidPath"));
        }

        [Test]
        public void AddTrigger_WithInvalidDatabasePath_DoesNotThrowException()
        {
            Assert.DoesNotThrow(
                () => MainTreeHandler.AddTrigger("NewTrigger", "InvalidPath"));
        }

        [Test]
        public void UpdateTableName_WithInvalidOldName_DoesNotThrowException()
        {
            Assert.DoesNotThrow(
                () => MainTreeHandler.UpdateTableName("InvalidOldName", "NewName", DATABASEPATH1));
        }

        [Test]
        public void UpdateViewName_WithInvalidOldName_DoesNotThrowException()
        {
            Assert.DoesNotThrow(
                () => MainTreeHandler.UpdateViewName("InvalidOldName", "NewName", DATABASEPATH1));
        }

        [Test]
        public void UpdateTriggerName_WithInvalidOldName_DoesNotThrowException()
        {
            Assert.DoesNotThrow(
                () => MainTreeHandler.UpdateTriggerName("InvalidOldName", "NewName", DATABASEPATH1));
        }
    }
}
