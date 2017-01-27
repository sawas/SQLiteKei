using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.Properties;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.CreatorWindow.Enums;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLiteKei.ViewModels.CreatorWindows.ColumnCreatorWindow
{
    public class ColumnCreatorViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private readonly string tableName;

        public string ColumnName { get; set; }

        private DataType selectedDataType;
        public DataType SelectedDataType
        {
            get { return selectedDataType; }
            set { selectedDataType = value; NotifyPropertyChanged(); }
        }

        public IEnumerable<DataType> DataTypes
        {
            get
            {
                return Enum.GetValues(typeof(DataType))
                    .Cast<DataType>();
            }
        }

        public bool IsNotNull { get; set; }

        public string DefaultValue { get; set; }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged(); }
        }

        public ColumnCreatorViewModel(string tableName)
        {
            this.tableName = tableName;
            SelectedDataType = DataType.Varchar;

            createCommand = new DelegateCommand(Create);
        }

        private void Create()
        {
            StatusInfo = string.Empty;

            using (var tableHandler = new TableHandler(Settings.Default.CurrentDatabase))
            {
                try
                {
                    tableHandler.AddColumn(tableName, ColumnName, SelectedDataType.ToString(), IsNotNull, DefaultValue);
                    StatusInfo = LocalisationHelper.GetString("ColumnCreator_StatusInfo_Success");
                }
                catch (Exception ex)
                {
                    logger.Warn("Failed to add new column to table '" + tableName + "'.", ex);
                    StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                }
            }
        }

        private DelegateCommand createCommand;

        public DelegateCommand CreateCommand { get { return createCommand; } }
    }
}
