using SQLiteKei.ViewModels.CreatorWindows.ViewCreatorWindow;
using System.Windows;

namespace SQLiteKei.Views.Windows.Creators
{
    /// <summary>
    /// Interaction logic for ViewCreator.xaml
    /// </summary>
    public partial class ViewCreator : Window
    {
        public ViewCreator()
        {
            DataContext = new ViewCreatorViewModel();
            InitializeComponent();
        }
    }
}
