using System;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using SQLiteKei.Util.Interfaces;
using SQLiteKei.Util;

namespace SQLiteKei.ViewModels.MainWindow.DBTreeView.DeleteStrategies
{
    /// <summary>
    /// A factory used to decide how the deletion of a tree view item will be handled.
    /// </summary>
    internal class DeleteStrategyFactory
    {
        /// <summary>
        /// Returns the appropriate strategy to delete 
        /// </summary>
        /// <param name="itemToDelete">The item to delete.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        internal IDeleteStrategy Create(TreeItem itemToDelete)
        {
            IDialogService dialogService = new DialogService();

            if (itemToDelete.GetType() == typeof(DatabaseItem))
                return new DatabaseDeleteStrategy(dialogService);
            if (itemToDelete.GetType() == typeof(TableItem))
                return new TableDeleteStrategy(dialogService);
            if (itemToDelete.GetType() == typeof(TriggerItem))
                return new TriggerDeleteStrategy(dialogService);
            if (itemToDelete.GetType() == typeof(ViewItem))
                return new ViewDeleteStrategy(dialogService);
            if (itemToDelete.GetType() == typeof(IndexItem))
                return new IndexDeleteStrategy(dialogService);

            throw new NotSupportedException();
        }
    }
}
