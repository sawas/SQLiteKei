using SQLiteKei.ViewModels.Base;

namespace SQLiteKei.ViewModels.CreatorWindows.TriggerCreatorWindow
{
    public class ColumnItem : NotifyingModel
    {
        public string ColumnName { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; NotifyPropertyChanged(); }
        }
    }
}
