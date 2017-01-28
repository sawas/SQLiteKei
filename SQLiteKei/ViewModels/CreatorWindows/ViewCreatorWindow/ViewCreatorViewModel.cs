using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SQLiteKei.ViewModels.CreatorWindows.ViewCreatorWindow
{
    /// <summary>
    /// ViewModel for the ViewCreator
    /// </summary>
    public class ViewCreatorViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private DatabaseSelectItem selectedDatabase;
        public DatabaseSelectItem SelectedDatabase
        {
            get { return selectedDatabase; }
            set { selectedDatabase = value; ValidateModel(); UpdateTableTree(); }
        }

        public IEnumerable<DatabaseSelectItem> Databases { get; set; }

        private string viewName;
        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; ValidateModel(); }
        }

        public bool IsIfNotExists { get; set; }

        private string sqlStatement;
        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; ValidateModel(); }
        }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged(); }
        }

        private bool isValidViewDefinition;
        public bool IsValidViewDefinition
        {
            get { return isValidViewDefinition; }
            set { isValidViewDefinition = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<TableTreeItem> Tables { get; set; }

        public ViewCreatorViewModel()
        {
            IsIfNotExists = true;

            Databases = MainTreeHandler.GetDatabaseSelectItems();
            Tables = new ObservableCollection<TableTreeItem>();

            createCommand = new DelegateCommand(Create);
        }

        private void UpdateTableTree()
        {
            Tables.Clear();

            IEnumerable<Table> tables;
            using (var dbHandler = new DatabaseHandler(selectedDatabase.DatabasePath))
            {
                tables = dbHandler.GetTables();
            }

            foreach (var table in tables)
            {
                var tableTreeItem = new TableTreeItem { DisplayName = table.Name };

                using (var tableHandler = new TableHandler(selectedDatabase.DatabasePath))
                {
                    var columns = tableHandler.GetColumns(table.Name);

                    foreach (var column in columns)
                    {
                        tableTreeItem.Columns.Add(new ColumnTreeItem { DisplayName = column.Name });
                    }
                }
                Tables.Add(tableTreeItem);
            }
        }

        private void ValidateModel()
        {
            IsValidViewDefinition = SelectedDatabase != null
                && !string.IsNullOrWhiteSpace(ViewName)
                && !string.IsNullOrWhiteSpace(SqlStatement);
        }

        private void Create()
        {
            StatusInfo = string.Empty;

            if (!sqlStatement.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase))
            {
                StatusInfo = LocalisationHelper.GetString("ViewGeneralTab_InvalidStatement");
                return;
            }

            var exectuableSql = QueryBuilder.CreateView(ViewName)
                .IfNotExists(IsIfNotExists)
                .As(sqlStatement)
                .Build();

            using (var dbHandler = new DatabaseHandler(selectedDatabase.DatabasePath))
            {
                try
                {
                    dbHandler.ExecuteNonQuery(exectuableSql);
                    StatusInfo = LocalisationHelper.GetString("ViewCreator_ViewCreateSuccess");
                    MainTreeHandler.AddView(viewName, selectedDatabase.DatabasePath);
                }
                catch(Exception ex)
                {
                    logger.Error("A view could not be created.", ex);
                    StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                }
            }
        } 

        private readonly DelegateCommand createCommand;

        public DelegateCommand CreateCommand { get { return createCommand; } }
    }
}
