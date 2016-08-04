using SQLiteKei.ViewModels.MainWindow.MainTabControl.Views;

using System.Windows.Controls;

namespace SQLiteKei.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ViewGeneralTabContent.xaml
    /// </summary>
    public partial class ViewGeneralTabContent : UserControl
    {
        public ViewGeneralTabContent(GeneralViewViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
