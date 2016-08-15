using log4net;

using SQLiteKei.Commands;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using SQLiteKei.Util;

using System;

namespace SQLiteKei.ViewModels.ElementRenameWindow
{
    public class ElementRenameViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

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
        }

        private void Rename()
        {
            StatusInfo = string.Empty;
            var itemType = originalElement.GetType();

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
        }

        private void RenameDatabase()
        {
            throw new NotImplementedException();
        }

        private void RenameTable()
        {
            throw new NotImplementedException();
        }

        private void RenameView()
        {
            throw new NotImplementedException();
        }

        private void RenameIndex()
        {
            throw new NotImplementedException();
        }

        private void RenameTrigger()
        {
            throw new NotImplementedException();
        }

        private DelegateCommand renameCommand;

        public DelegateCommand RenameCommand { get { return renameCommand; } }
    }
}
