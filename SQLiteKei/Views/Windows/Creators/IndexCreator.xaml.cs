using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.CreatorWindows.IndexCreatorWindow;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;

using System.Collections.Generic;
using System.Windows;

namespace SQLiteKei.Views.Windows.Creators
{
    /// <summary>
    /// Interaction logic for IndexCreator.xaml
    /// </summary>
    public partial class IndexCreator : Window
    {
        public IndexCreator(IEnumerable<TreeItem> databases)
        {
            var viewModel = new IndexCreatorViewModel();

            foreach (DatabaseItem database in databases)
            {
                viewModel.Databases.Add(new DatabaseSelectItem
                {
                    DatabaseName = database.DisplayName,
                    DatabasePath = database.DatabasePath
                });
            }

            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
