using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.CreatorWindows.TriggerCreatorWindow;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;

using System.Collections.Generic;
using System.Windows;


namespace SQLiteKei.Views.Windows.Creators
{
    /// <summary>
    /// Interaction logic for TriggerCreator.xaml
    /// </summary>
    public partial class TriggerCreator : Window
    {
        public TriggerCreator(IEnumerable<TreeItem> databases)
        {
            var viewModel = new TriggerCreatorViewModel();

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
