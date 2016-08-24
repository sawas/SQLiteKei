using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using SQLiteKei.ViewModels.QueryEditorWindow;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SQLiteKei.Views.Windows
{
    /// <summary>
    /// Interaction logic for QueryEditor.xaml
    /// </summary>
    public partial class QueryEditor : Window
    {
        public QueryEditor(IEnumerable<TreeItem> databases)
        {
            var viewModel = new QueryEditorViewModel();

            foreach(DatabaseItem database in databases)
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


        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var viewModel = DataContext as QueryEditorViewModel;

            viewModel.SelectedText = textBox.SelectedText;
        }
    }
}
