using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.ObjectModel;

namespace SQLiteKei.Util.Interfaces
{
    public interface ITreeSaveHelper
    {
        void Save(ObservableCollection<TreeItem> tree);
        ObservableCollection<TreeItem> Load();
    }
}
