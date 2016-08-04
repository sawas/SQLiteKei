using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.CreatorWindows.ViewCreatorWindow;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;

using System.Collections.Generic;
using System.Windows;

namespace SQLiteKei.Views.Windows.Creators
{
    /// <summary>
    /// Interaction logic for ViewCreator.xaml
    /// </summary>
    public partial class ViewCreator : Window
    {
        public ViewCreator(IEnumerable<TreeItem> databases)
        {
            var viewModel = new ViewCreatorViewModel();

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
