using System.Collections.Generic;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;

namespace SQLiteKei.ViewModels.MainWindow.DBTreeView.DeleteStrategies
{
    internal interface IDeleteStrategy
    {
        void Execute(ICollection<TreeItem> collection, TreeItem itemToDelete);
    }
}
