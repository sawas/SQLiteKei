using SQLiteKei.ViewModels.CreatorWindows.TableCreatorWindow;
using System.Windows;

namespace SQLiteKei.Views.Windows.Creators
{
    /// <summary>
    /// Interaction logic for TableCreator.xaml
    /// </summary>
    public partial class TableCreator : Window
    {
        public TableCreator()
        {
            DataContext = new TableCreatorViewModel();
            InitializeComponent();
        }
    }
}
