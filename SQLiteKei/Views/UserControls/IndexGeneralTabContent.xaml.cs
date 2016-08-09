using SQLiteKei.ViewModels.MainWindow.MainTabControl.Indexes;

using System.Windows.Controls;

namespace SQLiteKei.Views.UserControls
{
    /// <summary>
    /// Interaction logic for IndexGeneralTabContent.xaml
    /// </summary>
    public partial class IndexGeneralTabContent : UserControl
    {
        public IndexGeneralTabContent(GeneralIndexViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
