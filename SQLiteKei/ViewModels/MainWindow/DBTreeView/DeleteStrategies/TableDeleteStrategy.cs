using SQLiteKei.DataAccess.Database;
using SQLiteKei.Util.Interfaces;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.Generic;

namespace SQLiteKei.ViewModels.MainWindow.DBTreeView.DeleteStrategies
{
    internal class TableDeleteStrategy : DeleteStrategyBase, IDeleteStrategy
    {
        public TableDeleteStrategy(IDialogService dialogService) : base(dialogService)
        {
        }

        public void Execute(ICollection<TreeItem> collection, TreeItem itemToDelete)
        {
            var userAgrees = dialogService.AskForUserAgreement("MessageBox_TableDeleteWarning", "MessageBoxTitle_TableDeletion", itemToDelete.DisplayName);
            if (!userAgrees) return;

            using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
            {
                tableHandler.Drop(itemToDelete.DisplayName);
                RemoveItemFromHierarchy(collection, itemToDelete);
            }
        }
    }
}
