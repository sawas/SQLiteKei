using SQLiteKei.DataAccess.Database;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.Generic;

namespace SQLiteKei.ViewModels.MainWindow.DBTreeView.DeleteStrategies
{
    internal class IndexDeleteStrategy : DeleteStrategyBase, IDeleteStrategy
    {
        public void Execute(ICollection<TreeItem> collection, TreeItem itemToDelete)
        {
            var userAgrees = AskForUserAgreement("MessageBox_IndexDeleteWarning", "MessageBoxTitle_IndexDeletion", itemToDelete.DisplayName);
            if (!userAgrees) return;

            using (var indexHandler = new IndexHandler(Properties.Settings.Default.CurrentDatabase))
            {
                indexHandler.Drop(itemToDelete.DisplayName);
                RemoveItemFromHierarchy(collection, itemToDelete);
            }
        }
    }
}
