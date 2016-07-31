using log4net;

using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;

using System.Collections.ObjectModel;
using System.Linq;

namespace SQLiteKei.Helpers
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
        /// Adds the specified item to the main tree. The FolderType needs to be defined to place the item in the correct subfolder.
        /// </summary>
        /// <typeparam name="TFolderType">The type of the database sub folder where the item will be added.</typeparam>
        /// <typeparam name="TItemType">The type of the item.</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="targetDatabasePath">The target database path.</param>
        public static void AddItem<TFolderType, TItemType>(string itemName, string targetDatabasePath)
            where TFolderType : DirectoryItem
            where TItemType : TreeItem, new()
        {
            var database = GetDatabaseFromTree(targetDatabasePath);
            if (database == null) return;

            var folder = GetSubFolderOf<TFolderType>(database);
            if (folder == null) return;

            folder.Items.Add(new TItemType { DisplayName = itemName, DatabasePath = targetDatabasePath });
        }        

        private static DatabaseItem GetDatabaseFromTree(string targetDatabasePath)
        {
            var database = mainTree.SingleOrDefault(i => i.DatabasePath.Equals("path")) as DatabaseItem;

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
