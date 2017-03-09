using log4net;
using SQLiteKei.Util;
using SQLiteKei.Util.Interfaces;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SQLiteKei.ViewModels.MainWindow.DBTreeView.DeleteStrategies
{
    /// <summary>
    /// Base class for tree item deletion strategies. 
    /// </summary>
    internal class DeleteStrategyBase
    {
        private readonly ILog log = LogHelper.GetLogger();

        protected readonly IDialogService dialogService;

        protected DeleteStrategyBase(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        protected void RemoveItemFromHierarchy(ICollection<TreeItem> treeItems, TreeItem treeItem)
        {
            foreach (var item in treeItems)
            {
                if (item == treeItem)
                {
                    treeItems.Remove(item);
                    log.Info($"Removed item of type {item.GetType()} from tree hierarchy.");
                    break;
                }

                var directory = item as DirectoryItem;

                if (directory != null && directory.Items.Any())
                {
                    RemoveItemFromHierarchy(directory.Items, treeItem);
                }
            }
        }
    }
}
