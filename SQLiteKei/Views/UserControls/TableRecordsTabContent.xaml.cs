using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.ViewModels.MainWindow.MainTabControl.Tables;

using System.Windows.Controls;

namespace SQLiteKei.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TableRecordsTabContent.xaml
    /// </summary>
    public partial class TableRecordsTabContent : UserControl
    {
        public TableRecordsTabContent(RecordsTabViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
