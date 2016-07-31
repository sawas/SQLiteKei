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

        public static void AddTable(string tableName, string targetDatabasePath)
        {
            var database = GetDatabaseFromTree(targetDatabasePath);
            if (database == null) return;

            var folder = GetSubFolderOf<TableFolderItem>(database);
            if (folder == null) return;

            folder.Items.Add(new TableItem { DisplayName = tableName, DatabasePath = targetDatabasePath });
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
