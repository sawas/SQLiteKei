using SQLiteKei.ViewModels.QueryEditorWindow;
using System.Windows;
using System.Windows.Controls;

namespace SQLiteKei.Views.Windows
{
    /// <summary>
    /// Interaction logic for QueryEditor.xaml
    /// </summary>
    public partial class QueryEditor : Window
    {
        public QueryEditor()
        {
            DataContext = new QueryEditorViewModel();
            InitializeComponent();
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var viewModel = DataContext as QueryEditorViewModel;

            viewModel.SelectedText = textBox.SelectedText;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            comboBox.SelectedIndex = -1;
        }
    }
}
