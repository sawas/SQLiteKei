using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.Properties;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System;
using System.Collections.Generic;

namespace SQLiteKei.ViewModels.TableMigrator
{
    public class TableMigratorViewModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private string tableName;

        private MigrationType migrationType;

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

        public string StatusInfo { get; set; }

        public TableMigratorViewModel(IEnumerable<TreeItem> databases, string tableName, MigrationType migrationType)
        {
            this.tableName = tableName;
            this.migrationType = migrationType;
            sourceDatabase = Settings.Default.CurrentDatabase;

            if (migrationType == MigrationType.Copy)
                WindowTitle = LocalisationHelper.GetString("WindowTitle_TableMigrator_Copy", tableName);
            else
                WindowTitle = LocalisationHelper.GetString("WindowTitle_TableMigrator_Move", tableName);

            executeCommand = new DelegateCommand(Execute);

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

        private void Execute()
        {
            if (selectedDatabase == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(TargetTableName))
            {
                return;
            }

            try
            {
                if (migrationType == MigrationType.Copy)
                    Copy();
                else
                    Move();
            }
            catch (Exception ex)
            {

            }
            
        }

        private void Copy()
        {
            using (var sourceTableHandler = new TableHandler(sourceDatabase))
            using (var targetDbHandler = new DatabaseHandler(selectedDatabase.DatabasePath))
            {
                var originalTable = sourceTableHandler.GetTable(tableName);
                var createStatement = originalTable.CreateStatement.Replace(tableName, TargetTableName);

                targetDbHandler.ExecuteNonQuery(createStatement);

                MainTreeHandler.AddTable(TargetTableName, selectedDatabase.DatabasePath);

                if(!IsOnlyStructure)
                {

                }
            }
        }

        private void Move()
        {

        }

        private DelegateCommand executeCommand;

        public DelegateCommand ExecuteCommand { get { return executeCommand; } }
    }
}
