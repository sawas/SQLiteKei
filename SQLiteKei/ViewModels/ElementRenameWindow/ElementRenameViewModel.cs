using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using SQLiteKei.Util;

using System;
using System.Windows;
using System.IO;

namespace SQLiteKei.ViewModels.ElementRenameWindow
{
    public class ElementRenameViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private string windowTitle;
        public string WindowTitle
        {
            get { return windowTitle; }
            set { windowTitle = value; NotifyPropertyChanged("WindowTitle"); }
        }

        private TreeItem originalElement;

        public string NewName { get; set; }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        public ElementRenameViewModel(TreeItem treeItem)
        {
            originalElement = treeItem;
            renameCommand = new DelegateCommand(Rename);
            WindowTitle = LocalisationHelper.GetString("WindowTitle_RenameElement", treeItem.DisplayName);
        }

        private void Rename()
        {
            StatusInfo = string.Empty;
            var itemType = originalElement.GetType();

            try
            {
                if (itemType == typeof(DatabaseItem))
                    RenameDatabase();
                else if (itemType == typeof(TableItem))
                    RenameTable();
                else if (itemType == typeof(ViewItem))
                    RenameView();
                else if (itemType == typeof(IndexItem))
                    RenameIndex();
                else if (itemType == typeof(TriggerItem))
                    RenameTrigger();

                StatusInfo = LocalisationHelper.GetString("ElementRenameWindow_Success");
                originalElement.DisplayName = NewName;
                WindowTitle = LocalisationHelper.GetString("WindowTitle_RenameElement", NewName);
            }
            catch (Exception ex)
            {
                logger.Warn("Failed to rename the element '" + originalElement.DisplayName + "' from RenameWindow.", ex);
                var message = LocalisationHelper.GetString("MessageBox_NameChangeWarning", ex.Message);

                MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void RenameDatabase()
        {
            var originalFileDirectory = Path.GetDirectoryName(originalElement.DatabasePath);
            var originalFileEnding = Path.GetExtension(originalElement.DatabasePath);
            var newFileName = string.Format("{0}{1}", NewName, originalFileEnding);
            var newDatabasePath = Path.Combine(originalFileDirectory, newFileName);

            File.Move(originalElement.DatabasePath, newDatabasePath);
            MainTreeHandler.UpdateDatabase(originalElement.DatabasePath, newDatabasePath);
        }

        private void RenameTable()
        {
            using (var tableHandler = new TableHandler(originalElement.DatabasePath))
            {
                tableHandler.RenameTable(originalElement.DisplayName, NewName);
                MainTreeHandler.UpdateTableName(originalElement.DisplayName, NewName, originalElement.DatabasePath);
            }
        }

        private void RenameView()
        {
            using (var viewHandler = new ViewHandler(originalElement.DatabasePath))
            {
                viewHandler.UpdateViewName(originalElement.DisplayName, NewName);
                MainTreeHandler.UpdateViewName(originalElement.DisplayName, NewName, originalElement.DatabasePath);
            }
        }

        private void RenameIndex()
        {
            using (var indexHandler = new IndexHandler(originalElement.DatabasePath))
            {
                indexHandler.UpdateIndexName(originalElement.DisplayName, NewName);
                MainTreeHandler.UpdateIndexName(originalElement.DisplayName, NewName, originalElement.DatabasePath);
            }
        }

        private void RenameTrigger()
        {
            using (var triggerHandler = new TriggerHandler(originalElement.DatabasePath))
            {
                triggerHandler.UpdateTriggerName(originalElement.DisplayName, NewName);
                MainTreeHandler.UpdateTriggerName(originalElement.DisplayName, NewName, originalElement.DatabasePath);
            }
        }

        private DelegateCommand renameCommand;

        public DelegateCommand RenameCommand { get { return renameCommand; } }
    }
}
