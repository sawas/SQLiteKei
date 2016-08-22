using SQLiteKei.ViewModels.TableMigrator;

using System.Windows;

namespace SQLiteKei.Views.Windows
{
    /// <summary>
    /// Interaction logic for TableMigrator.xaml
    /// </summary>
    public partial class TableMigrator : Window
    {
        public TableMigrator(TableMigratorViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
