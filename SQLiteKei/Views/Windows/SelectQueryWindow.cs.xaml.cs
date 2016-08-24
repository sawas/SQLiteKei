using SQLiteKei.ViewModels.SelectQueryWindow;

using System.Windows;

namespace SQLiteKei.Views.Windows
{
    /// <summary>
    /// Interaction logic for GenerateSelectQueryWindow.xaml
    /// </summary>
    public partial class SelectQueryWindow : Window
    {
        public SelectQueryWindow(SelectQueryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
