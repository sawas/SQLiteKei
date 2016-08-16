using log4net;

using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Mapping;
using System.Collections.ObjectModel;
using System.Linq;

namespace SQLiteKei.Util
{
    /// <summary>
    /// A class that is used to update elements on the main tree from anywhere in the application.
    /// </summary>
    public static class MainTreeHandler
    {
        private static ILog logger = LogHelper.GetLogger();

        private static ObservableCollection<TreeItem> mainTree;

        /// <summary>
        /// Registers the main tree to this static class.
        /// </summary>
        /// <param name="tree">The tree.</param>
        public static void Register(ObservableCollection<TreeItem> tree)
        {
            mainTree = tree;
            logger.Info("Registered main tree on MainTreeHandler");
        }

        /// <summary>
        /// Updates the database information.
        /// </summary>
        /// <param name="oldPath">The old path.</param>
        /// <param name="newPath">The new path.</param>
        public static void UpdateDatabase(string oldPath, string newPath)
        {
            var mapper = new SchemaToViewModelMapper();
            var database = mainTree.SingleOrDefault(x => x.DatabasePath.Equals(oldPath));

            mainTree.Remove(database);
            mainTree.Add(mapper.MapSchemaToViewModel(newPath));
        }

        /// <summary>
        /// Adds a table item to the specified target database.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="targetDatabasePath">The target database path.</param>
        public static void AddTable(string tableName, string targetDatabasePath)
        {
            AddItem<TableFolderItem, TableItem>(tableName, targetDatabasePath);
        }

        /// <summary>
        /// Adds a view item to the specified target database.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="targetDatabasePath">The target database path.</param>
        public static void AddView(string viewName, string targetDatabasePath)
        {
            AddItem<ViewFolderItem, ViewItem>(viewName, targetDatabasePath);
        }

        /// <summary>
        /// Adds an index item to the specified target database.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="targetDatabasePath">The target database path.</param>
        public static void AddIndex(string indexName, string targetDatabasePath)
        {
            AddItem<IndexFolderItem, IndexItem>(indexName, targetDatabasePath);
        }

        /// <summary>trigger item to the specified target database.
        /// </summary>
        /// <param name="triggerName">Name of the trigger.</param>
        /// <param name="targetDatabasePath">The target database path.</param>
        public static void AddTrigger(string triggerName, string targetDatabasePath)
        {
            AddItem<TriggerFolderItem, TriggerItem>(triggerName, targetDatabasePath);
        }

        /// <summary>
        /// Adds the specified item to the main tree. The FolderType needs to be defined to place the item in the correct subfolder.
        /// </summary>
        /// <typeparam name="TFolderType">The type of the database sub folder where the item will be added.</typeparam>
        /// <typeparam name="TItemType">The type of the item.</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="targetDatabasePath">The target database path.</param>
        private static void AddItem<TFolderType, TItemType>(string itemName, string targetDatabasePath)
            where TFolderType : DirectoryItem
            where TItemType : TreeItem, new()
        {
            var database = GetDatabaseFromTree(targetDatabasePath);
            if (database == null) return;

            var folder = GetSubFolderOf<TFolderType>(database);
            if (folder == null) return;

            var result = folder.Items.SingleOrDefault(i => i.DisplayName.Equals(itemName));
            if (result != null) return;

            folder.Items.Add(new TItemType { DisplayName = itemName, DatabasePath = targetDatabasePath });
        }

        /// <summary>
        /// Updates the name of the table tree item on the specified database.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="targetDatabasePath">The target database path.</param>
        public static void UpdateTableName(string oldName, string newName, string targetDatabasePath)
        {
            UpdateItemName<TableFolderItem>(oldName, newName, targetDatabasePath);
        }

        /// <summary>
        /// Updates the name of the view tree item on the specified database.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="targetDatabasePath">The target database path.</param>
        public static void UpdateViewName(string oldName, string newName, string targetDatabasePath)
        {
            UpdateItemName<ViewFolderItem>(oldName, newName, targetDatabasePath);
        }

        /// <summary>
        /// Updates the name of the index tree item on the specified database.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="targetDatabasePath">The target database path.</param>
        public static void UpdateIndexName(string oldName, string newName, string targetDatabasePath)
        {
            UpdateItemName<IndexFolderItem>(oldName, newName, targetDatabasePath);
        }

        /// <summary>
        /// Updates the name of the trigger tree item on the specified database.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="targetDatabasePath">The target database path.</param>
        public static void UpdateTriggerName(string oldName, string newName, string targetDatabasePath)
        {
            UpdateItemName<TriggerFolderItem>(oldName, newName, targetDatabasePath);
        }

        private static void UpdateItemName<TFolderType>(string oldName, string newName, string targetDatabasePath)
            where TFolderType : DirectoryItem
        {
            var database = GetDatabaseFromTree(targetDatabasePath);
            if (database == null) return;

            var folder = GetSubFolderOf<TFolderType>(database);
            if (folder == null) return;

            var targetItem = folder.Items.SingleOrDefault(i => i.DisplayName.Equals(oldName));

            if (targetItem == null)
            {
                logger.Error("Could not update the name of a tree item.");
                return;
            }

            targetItem.DisplayName = newName;
        }    

        private static DatabaseItem GetDatabaseFromTree(string targetDatabasePath)
        {
            var database = mainTree.SingleOrDefault(i => i.DatabasePath.Equals(targetDatabasePath)) as DatabaseItem;

            if (database == null)
                logger.Error("Could not add object to database tree view. The specified database could not be found: " + targetDatabasePath);

            return database;
        }

        private static TType GetSubFolderOf<TType>(DatabaseItem db)
            where TType : DirectoryItem
        {
            var folder = db.Items.SingleOrDefault(i => i.GetType() == typeof(TType)) as TType;

            if (folder == null)
                logger.Error("Could not find a tree view subfolder item of type " + typeof(TType));

            return folder;
        }
    }
}
