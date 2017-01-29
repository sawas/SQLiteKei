using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.Generic;
using System.Linq;

namespace SQLiteKei.UnitTests.TestUtils
{
    internal static class TreeSearcher
    {
        public static bool DatabaseHoldsItem<TType>(ICollection<TreeItem> sourceCollection, string itemName, string databasePath)
            where TType : DirectoryItem
        {
            var db = GetDatabaseFromTree(sourceCollection, databasePath);
            var folder = GetSubFolderOf<TType>(db);
            var item = folder.Items.SingleOrDefault(i => i.DisplayName.Equals(itemName));

            return item == null ? false : true;
        }

        private static DatabaseItem GetDatabaseFromTree(ICollection<TreeItem> sourceCollection, string targetDatabasePath)
        {
            return sourceCollection.SingleOrDefault(i => i.DatabasePath.Equals(targetDatabasePath)) as DatabaseItem;
        }

        private static TType GetSubFolderOf<TType>(DatabaseItem db)
            where TType : DirectoryItem
        {
            return db.Items.SingleOrDefault(i => i.GetType() == typeof(TType)) as TType;
        }
    }
}
