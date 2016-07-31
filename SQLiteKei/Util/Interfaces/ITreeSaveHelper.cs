using System.Collections.ObjectModel;

using SQLiteKei.ViewModels.DBTreeView.Base;

namespace SQLiteKei.Util.Interfaces
{
    public interface ITreeSaveHelper
    {
        void Save(ObservableCollection<TreeItem> tree);
        ObservableCollection<TreeItem> Load();
    }
}
