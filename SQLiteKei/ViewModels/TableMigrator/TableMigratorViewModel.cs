using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.Properties;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;

using System;
using System.Collections.Generic;
using System.Data;

namespace SQLiteKei.ViewModels.TableMigrator
{
    public class TableMigratorViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private string tableName;

        public string WindowTitle { get; set; }

        private string sourceDatabase;

        private DatabaseSelectItem selectedDatabase;
        public DatabaseSelectItem SelectedDatabase
        {
            get { return selectedDatabase; }
            set { selectedDatabase = value; }
        }

        public List<DatabaseSelectItem> Databases { get; set; }

        public string TargetTableName { get; set; }

        public bool IsOnlyStructure { get; set; }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        public TableMigratorViewModel(IEnumerable<TreeItem> databases, string tableName)
        {
            this.tableName = TargetTableName = tableName;
            sourceDatabase = Settings.Default.CurrentDatabase;
            WindowTitle = LocalisationHelper.GetString("WindowTitle_TableMigrator_Copy", tableName);

            executeCommand = new DelegateCommand(Copy);

            Databases = new List<DatabaseSelectItem>();

            foreach (DatabaseItem database in databases)
            {
                if (!database.DatabasePath.Equals(sourceDatabase))
                {
                    Databases.Add(new DatabaseSelectItem
                    {
                        DatabaseName = database.DisplayName,
                        DatabasePath = database.DatabasePath
                    });
                }
            }
        }

        private void Copy()
        {
            if (selectedDatabase == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(TargetTableName))
            {
                return;
            }

            CopyTable();
        }

        private void CopyTable()
        {
            logger.Info("Copying table '" + tableName + "' as '" + TargetTableName
                + "' from database " + Environment.NewLine + sourceDatabase + Environment.NewLine
                + " to database " + Environment.NewLine + selectedDatabase.DatabasePath);

            bool isSuccessfullTableCopy = false;

            try
            {
                using (var sourceTableHandler = new TableHandler(sourceDatabase))
                using (var targetDbHandler = new DatabaseHandler(selectedDatabase.DatabasePath))
                {
                    var originalTable = sourceTableHandler.GetTable(tableName);
                    var createStatement = originalTable.CreateStatement.Replace(tableName, TargetTableName);

                    targetDbHandler.ExecuteNonQuery(createStatement);

                    MainTreeHandler.AddTable(TargetTableName, selectedDatabase.DatabasePath);

                    StatusInfo = LocalisationHelper.GetString("TableMigrator_CopySuccess");
                    isSuccessfullTableCopy = true;
                    logger.Info("Successfully copied table structure to target database.");
                }
            }
            catch (Exception ex)
            {
                logger.Error("A table could not be copied.", ex);
                StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
            }

            if (!IsOnlyStructure && isSuccessfullTableCopy)
            {
                CopyValues();
            }
        }

        private void CopyValues()
        {
            logger.Info("Copying values.");

            try
            {
                using (var sourceTableHandler = new TableHandler(sourceDatabase))
                using (var targetDbHandler = new DatabaseHandler(selectedDatabase.DatabasePath))
                {
                    var records = sourceTableHandler.GetRows(tableName);
                    var values = new List<string>();

                    foreach (DataRow row in records)
                    {
                        values.Clear();

                        foreach (var value in row.ItemArray)
                        {
                            values.Add(value.ToString());
                        }

                        var command = QueryBuilder.InsertInto(TargetTableName).Values(values).Build();
                        targetDbHandler.ExecuteNonQuery(command);
                    }
                }
                StatusInfo = LocalisationHelper.GetString("TableMigrator_CopySuccess");
                logger.Info("Successfully copied values");
            }
            catch(Exception ex)
            {
                logger.Error("Failed to copy table values.", ex);
                StatusInfo = LocalisationHelper.GetString("TableMigrator_CopySemiSuccess");
            }
        }

        private DelegateCommand executeCommand;

        public DelegateCommand ExecuteCommand { get { return executeCommand; } }
    }
}
