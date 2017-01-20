using System.Collections.ObjectModel;

namespace SQLiteKei.ViewModels.MainWindow.DBTreeView.Base
{
    /// <summary>
    /// The base class for tree items that contain a list of child items.
    /// </summary>
    public abstract class DirectoryItem : TreeItem
    {
        public ObservableCollection<TreeItem> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryItem"/> class.
        /// </summary>
        protected DirectoryItem()
        {
            Items = new ObservableCollection<TreeItem>();
        }
    }
}