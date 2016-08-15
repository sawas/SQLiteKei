using SQLiteKei.Commands;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;

namespace SQLiteKei.ViewModels.ElementRenameWindow
{
    public class ElementRenameViewModel : NotifyingModel
    {
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
        }

        private DelegateCommand renameCommand;

        public DelegateCommand RenameCommand { get { return renameCommand; } }
    }
}
