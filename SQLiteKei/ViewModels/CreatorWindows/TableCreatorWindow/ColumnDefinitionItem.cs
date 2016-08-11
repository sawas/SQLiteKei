using System;
using System.Collections.Generic;
using System.Linq;

using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.CreatorWindows.TableCreatorWindow.Enums;

namespace SQLiteKei.ViewModels.CreatorWindows.TableCreatorWindow
{
    public class ColumnDefinitionItem : NotifyingModel
    {
        private string columnName;
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; NotifyPropertyChanged("ColumnName"); }
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
                NotifyPropertyChanged("IsPrimary");
            }
        }

        private bool isNotNull;
        public bool IsNotNull
        {
            get { return isNotNull; }
            set { isNotNull = value; NotifyPropertyChanged("IsNotNull"); }
        }

        private DataType dataType;
        public DataType DataType
        {
            get { return dataType; }
            set { dataType = value; NotifyPropertyChanged("DataType"); }
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
            set { defaultValue = value; NotifyPropertyChanged("DefaultValue"); }
        }

        public ColumnDefinitionItem()
        {
            columnName = "<Name>";
        }
    }
}
