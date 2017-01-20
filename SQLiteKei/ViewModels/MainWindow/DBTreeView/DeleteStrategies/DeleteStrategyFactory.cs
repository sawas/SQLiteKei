using System;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;

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
        /// <exception cref="System.NotSupportedException"></exception>
        internal IDeleteStrategy Create(TreeItem itemToDelete)
        {
            if (itemToDelete.GetType() == typeof(DatabaseItem))
                return new DatabaseDeleteStrategy();
            if (itemToDelete.GetType() == typeof(TableItem))
                return new TableDeleteStrategy();
            if (itemToDelete.GetType() == typeof(TriggerItem))
                return new TriggerDeleteStrategy();
            if (itemToDelete.GetType() == typeof(ViewItem))
                return new ViewDeleteStrategy();
            if (itemToDelete.GetType() == typeof(IndexItem))
                return new IndexDeleteStrategy();

            throw new NotSupportedException();
        }
    }
}
