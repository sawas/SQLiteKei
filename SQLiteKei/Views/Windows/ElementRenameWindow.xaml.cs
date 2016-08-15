using SQLiteKei.ViewModels.ElementRenameWindow;

using System.Windows;

namespace SQLiteKei.Views.Windows
{
    /// <summary>
    /// Interaction logic for ElementRenameWindow.xaml
    /// </summary>
    public partial class ElementRenameWindow : Window
    {
        public ElementRenameWindow(ElementRenameViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
