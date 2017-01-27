using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.CreatorWindow.Enums;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLiteKei.ViewModels.CreatorWindows.TableCreatorWindow
{
    public class ColumnDefinitionItem : NotifyingModel
    {
        private string columnName;
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; NotifyPropertyChanged(); }
        }

        private bool isPrimary;
        public bool IsPrimary
        {
            get { return isPrimary; }
            set
            {
                isPrimary = value;
                if (value)
                    IsNotNull = true;
                NotifyPropertyChanged();
            }
        }

        private bool isNotNull;
        public bool IsNotNull
        {
            get { return isNotNull; }
            set { isNotNull = value; NotifyPropertyChanged(); }
        }

        private DataType dataType;
        public DataType DataType
        {
            get { return dataType; }
            set { dataType = value; NotifyPropertyChanged(); }
        }

        public IEnumerable<DataType> DataTypes
        {
            get
            {
                return Enum.GetValues(typeof (DataType))
                    .Cast<DataType>();
            }
        }

        private string defaultValue;
        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; NotifyPropertyChanged(); }
        }

        public ColumnDefinitionItem()
        {
            columnName = "<Name>";
            DataType = DataType.Varchar;
        }
    }
}
