﻿using SQLiteKei.DataAccess.Database;
using SQLiteKei.Util.Interfaces;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.Generic;

namespace SQLiteKei.ViewModels.MainWindow.DBTreeView.DeleteStrategies
{
    internal class ViewDeleteStrategy : DeleteStrategyBase, IDeleteStrategy
    {
        public ViewDeleteStrategy(IDialogService dialogService) : base(dialogService)
        {
        }

        public void Execute(ICollection<TreeItem> collection, TreeItem itemToDelete)
        {
            var userAgrees = dialogService.AskForUserAgreement("MessageBox_ViewDeleteWarning", "MessageBoxTitle_ViewDeletion", itemToDelete.DisplayName);
            if (!userAgrees) return;

            using (var viewHandler = new ViewHandler(Properties.Settings.Default.CurrentDatabase))
            {
                viewHandler.Drop(itemToDelete.DisplayName);
                RemoveItemFromHierarchy(collection, itemToDelete);
            }
        }
    }
}
