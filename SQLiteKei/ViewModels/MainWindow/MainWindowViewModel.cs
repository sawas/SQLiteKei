using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.Util;
using SQLiteKei.Util.Interfaces;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.DeleteStrategies;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Mapping;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
            documentationCommand = new DelegateCommand(OpenDocumentation);
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

        internal void Delete(TreeItem treeItem)
        {
            var factory = new DeleteStrategyFactory();

            try
            {
                var deleteStrategy = factory.Create(treeItem);
                deleteStrategy.Execute(treeViewItems, treeItem);
            }
            catch (Exception ex)
            {
                log.Error("Failed to delete item '" + treeItem.DisplayName + "' of type  " + treeItem.GetType() + ".", ex);
                var statusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                StatusBarInfo = statusInfo;
            }
            
        }

        private void OpenDocumentation()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Documentation.pdf");
            try
            {
                Process.Start(path);
            }
            catch (Exception ex)
            {
                log.Error("Failed to open documentation.", ex);
            }
        }

        private DelegateCommand documentationCommand;

        public DelegateCommand DocumentationCommand { get { return documentationCommand; } }
    }
}
