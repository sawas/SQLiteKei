using log4net;
using SQLiteKei.Util;
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

        protected bool AskForUserAgreement(string messageBodyKey, string messageTitleKey, string itemName)
        {
            var fullMessage = LocalisationHelper.GetString(messageBodyKey, itemName);
            var result = MessageBox.Show(fullMessage, LocalisationHelper.GetString(messageTitleKey), MessageBoxButton.YesNo, MessageBoxImage.Warning);

            return result == MessageBoxResult.Yes;
        }

        protected void RemoveItemFromHierarchy(ICollection<TreeItem> treeItems, TreeItem treeItem)
        {
            foreach (var item in treeItems)
            {
                if (item == treeItem)
                {
                    treeItems.Remove(item);
                    log.Info(string.Format("Removed item of type {0} from tree hierarchy.", item.GetType()));
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
