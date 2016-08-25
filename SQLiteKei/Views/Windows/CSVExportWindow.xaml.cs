using SQLiteKei.ViewModels.CSVExportWindow;

using System.Windows;

namespace SQLiteKei.Views.Windows
{
    /// <summary>
    /// Interaction logic for CSVExportWindow.xaml
    /// </summary>
    public partial class CSVExportWindow : Window
    {
        public CSVExportWindow(CSVExportViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
