using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Helpers;
using SQLiteKei.Helpers.Interfaces;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.DBTreeView.Mapping;

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SQLiteKei.ViewModels.MainWindow
{
    public class MainWindowViewModel : NotifyingModel
    {
        private readonly ITreeSaveHelper treeSaveHelper;

        private readonly ILog log = LogHelper.GetLogger();

        public TreeItem SelectedItem { get; set; }

        private ObservableCollection<TreeItem> treeViewItems;
        public ObservableCollection<TreeItem> TreeViewItems
        {
            get { return treeViewItems; }
            set { treeViewItems = value; NotifyPropertyChanged("TreeViewItems"); }
        }

        private string statusBarInfo;
        public string StatusBarInfo
        {
            get { return statusBarInfo; }
            set { statusBarInfo = value; NotifyPropertyChanged("StatusBarInfo"); }
        }

        public MainWindowViewModel(ITreeSaveHelper treeSaveHelper)
        {
            this.treeSaveHelper = treeSaveHelper;
            TreeViewItems = treeSaveHelper.Load();

            MainTreeHandler.Register(TreeViewItems);
        }

        public void OpenDatabase(string databasePath)
        {
            if (TreeViewItems.Any(x => x.DatabasePath.Equals(databasePath))) 
                return;

            var schemaMapper = new SchemaToViewModelMapper();
            DatabaseItem databaseItem = schemaMapper.MapSchemaToViewModel(databasePath);

            TreeViewItems.Add(databaseItem);

            log.Info("Opened database '" + databaseItem.DisplayName + "'.");
        }

        public void CloseDatabase(string databasePath)
        {
            var db = TreeViewItems.SingleOrDefault(x => x.DatabasePath.Equals(databasePath));
            TreeViewItems.Remove(db);

            log.Info("Closed database '" + db.DisplayName + "'.");
        }

        public void RemoveItemFromTree(TreeItem treeItem)
        {
            RemoveItemFromHierarchy(TreeViewItems, treeItem);
        }

        private void RemoveItemFromHierarchy(ICollection<TreeItem> treeItems, TreeItem treeItem)
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

        public void RefreshTree()
        {
            log.Info("Refreshing the database tree.");
            var databasePaths = TreeViewItems.Select(x => x.DatabasePath).ToList();
            TreeViewItems.Clear();

            var schemaMapper = new SchemaToViewModelMapper();
            foreach (var path in databasePaths)
            {
                TreeViewItems.Add(schemaMapper.MapSchemaToViewModel(path));
            }
        }

        public void SaveTree()
        {
            treeSaveHelper.Save(TreeViewItems);
        }

        internal void EmptyTable(string tableName)
        {
            var message = LocalisationHelper.GetString("MessageBox_EmptyTable", tableName);
            var messageTitle = LocalisationHelper.GetString("MessageBoxTitle_EmptyTable");
            var result = MessageBox.Show(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
            {
                try
                {
                    tableHandler.EmptyTable(tableName);
                }
                catch (Exception ex)
                {
                    log.Error("Failed to empty table" + tableName, ex);
                    StatusBarInfo = ex.Message;
                }
            }
        }

        internal void DeleteTable(TableItem tableItem)
        {
            var message = LocalisationHelper.GetString("MessageBox_TableDeleteWarning", tableItem.DisplayName);
            var result = MessageBox.Show(message, LocalisationHelper.GetString("MessageBoxTitle_TableDeletion"), MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
                {
                    tableHandler.DropTable(tableItem.DisplayName);
                    RemoveItemFromTree(tableItem);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to delete table '" + tableItem.DisplayName + "'.", ex);
                var statusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                StatusBarInfo = statusInfo;
            }
        }

        public void DeleteView(ViewItem viewItem)
        {
            var message = LocalisationHelper.GetString("MessageBox_ViewDeleteWarning", viewItem.DisplayName);
            var result = MessageBox.Show(message, LocalisationHelper.GetString("MessageBoxTitle_ViewDeletion"), MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                using (var viewHandler = new ViewHandler(Properties.Settings.Default.CurrentDatabase))
                {
                    viewHandler.DropView(viewItem.DisplayName);
                    RemoveItemFromTree(viewItem);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to delete view '" + viewItem.DisplayName + "'.", ex);
                var statusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                StatusBarInfo = statusInfo;
            }
        }

        internal void DeleteTrigger(TriggerItem triggerItem)
        {
            var message = LocalisationHelper.GetString("MessageBox_TriggerDeleteWarning", triggerItem.DisplayName);
            var result = MessageBox.Show(message, LocalisationHelper.GetString("MessageBoxTitle_TriggerDeletion"), MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                using (var triggerHandler = new TriggerHandler(Properties.Settings.Default.CurrentDatabase))
                {
                    triggerHandler.DropTrigger(triggerItem.DisplayName);
                    RemoveItemFromTree(triggerItem);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to delete trigger '" + triggerItem.DisplayName + "'.", ex);
                var statusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                StatusBarInfo = statusInfo;
            }
        }
    }
}
