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

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if(e.EditAction == DataGridEditAction.Commit)
            {
                var currentRow = SelectResultGrid.SelectedCells;
                var columnName = e.Column.Header.ToString();
                var newValue = ((TextBox)e.EditingElement).Text;

                ((RecordsTabViewModel)DataContext).UpdateValue(currentRow, columnName, newValue);
            }
        }
    }
}
