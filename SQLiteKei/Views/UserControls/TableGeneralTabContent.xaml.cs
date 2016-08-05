using System.Windows.Controls;

using SQLiteKei.ViewModels.MainWindow.MainTabControl.Tables;

namespace SQLiteKei.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TableGeneralTabContent.xaml
    /// </summary>
    public partial class TableGeneralTabContent : UserControl
    {
        public TableGeneralTabContent(GeneralTableViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
