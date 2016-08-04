using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.CreatorWindows.TableCreatorWindow;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.Generic;
using System.Windows;

namespace SQLiteKei.Views.Windows.Creators
{
    /// <summary>
    /// Interaction logic for TableCreator.xaml
    /// </summary>
    public partial class TableCreator : Window
    {
        public TableCreator(IEnumerable<TreeItem> databases)
        {
            var viewModel = new TableCreatorViewModel();

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
