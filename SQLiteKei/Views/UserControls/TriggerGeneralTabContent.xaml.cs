using SQLiteKei.ViewModels.MainWindow.MainTabControl.Trigger;

using System.Windows.Controls;

namespace SQLiteKei.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TriggerGeneralTabContent.xaml
    /// </summary>
    public partial class TriggerGeneralTabContent : UserControl
    {
        public TriggerGeneralTabContent(GeneralTriggerViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
