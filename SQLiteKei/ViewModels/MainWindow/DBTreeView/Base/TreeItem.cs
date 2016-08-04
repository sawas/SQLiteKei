using SQLiteKei.ViewModels.Base;

namespace SQLiteKei.ViewModels.MainWindow.DBTreeView.Base
{
    /// <summary>
    /// The base class for tree view items.
    /// </summary>
    public abstract class TreeItem : NotifyingModel
    {
        private string displayName;

        /// <summary>
        /// Gets or sets the name that is displayed in the tree view.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; NotifyPropertyChanged("DisplayName"); }
        }

        /// <summary>
        /// Gets or sets the database path. This is used to determine the current database context when a tree item is clicked.
        /// </summary>
        /// <value>
        /// The database path.
        /// </value>
        public string DatabasePath { get; set; }
    }
}