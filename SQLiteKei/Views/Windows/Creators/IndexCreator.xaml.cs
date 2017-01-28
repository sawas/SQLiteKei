using SQLiteKei.ViewModels.CreatorWindows.IndexCreatorWindow;
using System.Windows;

namespace SQLiteKei.Views.Windows.Creators
{
    /// <summary>
    /// Interaction logic for IndexCreator.xaml
    /// </summary>
    public partial class IndexCreator : Window
    {
        public IndexCreator()
        {
            DataContext = new IndexCreatorViewModel();
            InitializeComponent();
        }
    }
}
