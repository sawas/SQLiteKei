using SQLiteKei.Util.Interfaces;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.Generic;
using System.IO;

namespace SQLiteKei.ViewModels.MainWindow.DBTreeView.DeleteStrategies
{
    internal class DatabaseDeleteStrategy : DeleteStrategyBase, IDeleteStrategy
    {
        public DatabaseDeleteStrategy(IDialogService dialogService) : base(dialogService)
        {
        }

        public void Execute(ICollection<TreeItem> collection, TreeItem itemToDelete)
        {
            var userAgrees = dialogService.AskForUserAgreement("MessageBox_DatabaseDeleteWarning", "MessageBoxTitle_DatabaseDeletion", itemToDelete.DisplayName);
            if (!userAgrees) return;

            if (!File.Exists(itemToDelete.DatabasePath))
                    throw new FileNotFoundException("Database file could not be found.");

            File.Delete(itemToDelete.DatabasePath);
            RemoveItemFromHierarchy(collection, itemToDelete);
        }
    }
}
