using SQLiteKei.ViewModels.CreatorWindow.Enums;

namespace SQLiteKei.ViewModels.CreatorWindows.ColumnCreatorWindow
{
    public class ColumnCreatorViewModel
    {
        private readonly string tableName;

        public string ColumnName { get; set; }

        public DataType DataType { get; set; }

        public ColumnCreatorViewModel(string tableName)
        {
            this.tableName = tableName;
        }
    }
}
