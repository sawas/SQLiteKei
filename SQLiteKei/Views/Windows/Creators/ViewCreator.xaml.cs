using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.DBTreeView;

using System.Collections.Generic;
using System.Windows;
using SQLiteKei.ViewModels.CreatorWindows.ViewCreatorWindow;

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
