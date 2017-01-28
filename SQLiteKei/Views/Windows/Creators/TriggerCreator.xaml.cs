using SQLiteKei.ViewModels.CreatorWindows.TriggerCreatorWindow;
using System.Windows;


namespace SQLiteKei.Views.Windows.Creators
{
    /// <summary>
    /// Interaction logic for TriggerCreator.xaml
    /// </summary>
    public partial class TriggerCreator : Window
    {
        public TriggerCreator()
        {
            DataContext = new TriggerCreatorViewModel();
            InitializeComponent();
        }
    }
}
