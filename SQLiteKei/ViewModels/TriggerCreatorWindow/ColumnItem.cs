using SQLiteKei.ViewModels.Base;

namespace SQLiteKei.ViewModels.TriggerCreatorWindow
{
    public class ColumnItem : NotifyingModel
    {
        public string ColumnName { get; set; }
        public bool IsSelected { get; set; }
    }
}
