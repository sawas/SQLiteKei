using SQLiteKei.ViewModels.MainWindow.MainTabControl.Databases;

using System.Windows.Controls;

namespace SQLiteKei.Views.UserControls
{
    /// <summary>
    /// Interaction logic for DatabaseSettingsTabContent.xaml
    /// </summary>
    public partial class DatabaseSettingsTabContent : UserControl
    {
        public DatabaseSettingsTabContent(DatabaseSettingsViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
