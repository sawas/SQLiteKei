using System.Collections.Generic;

namespace SQLiteKei.ViewModels.CreatorWindows.ViewCreatorWindow
{
    public class TableTreeItem
    {
        public string DisplayName { get; set; }

        public List<ColumnTreeItem> Columns { get; set; }

        public TableTreeItem()
        {
            Columns = new List<ColumnTreeItem>();
        }
    }
}
