using SQLiteKei.ViewModels.CreatorWindows.ColumnCreatorWindow;

using System.Windows;

namespace SQLiteKei.Views.Windows.Creators
{
    /// <summary>
    /// Interaction logic for ColumnCreator.xaml
    /// </summary>
    public partial class ColumnCreator : Window
    {
        public ColumnCreator(ColumnCreatorViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
