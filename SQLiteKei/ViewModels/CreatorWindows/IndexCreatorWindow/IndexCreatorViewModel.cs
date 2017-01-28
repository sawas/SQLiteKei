using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.DataAccess.QueryBuilders.Enums;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System;
using System.ComponentModel;
using System.Linq;

namespace SQLiteKei.ViewModels.CreatorWindows.IndexCreatorWindow
{
    public class IndexCreatorViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private DatabaseSelectItem selectedDatabase;
        public DatabaseSelectItem SelectedDatabase
        {
            get { return selectedDatabase; }
            set { selectedDatabase = value; UpdateAvailableTables(); }
        }

        public IEnumerable<DatabaseSelectItem> Databases { get; set; }

        private string indexName;
        public string IndexName
        {
            get { return indexName; }
            set { indexName = value; UpdateModel(); }
        }

        private bool isIfNotExists;
        public bool IsIfNotExists
        {
            get { return isIfNotExists; }
            set { isIfNotExists = value; UpdateSql(); }
        }

        private string selectedTable;
        public string SelectedTable
        {
            get { return selectedTable; }
            set { selectedTable = value; UpdateAvailableColumns(); UpdateModel(); }
        }

        public ObservableCollection<string> Tables { get; set; }

        private bool isUnique;
        public bool IsUnique
        {
            get { return isUnique; }
            set { isUnique = value; UpdateSql(); }
        }

        public ObservableCollection<ColumnItem> Columns { get; set; }

        private string whereStatement;
        public string WhereStatement
        {
            get { return whereStatement; }
            set { whereStatement = value; UpdateSql(); }
        }

        private string sqlStatement;
        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; NotifyPropertyChanged(); }
        }

        private bool isValidModel;
        public bool IsValidModel
        {
            get { return isValidModel; }
            set { isValidModel = value; NotifyPropertyChanged(); }
        }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged(); }
        }

        public IndexCreatorViewModel()
        {
            Databases = MainTreeHandler.GetDatabaseSelectItems();
            Tables = new ObservableCollection<string>();
            Columns = new ObservableCollection<ColumnItem>();
            Columns.CollectionChanged += CollectionContentChanged;

            createCommand = new DelegateCommand(Create);
        }

        private void CollectionContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (NotifyingModel item in e.OldItems)
                {
                    item.PropertyChanged -= CollectionItemPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (NotifyingModel item in e.NewItems)
                {
                    item.PropertyChanged += CollectionItemPropertyChanged;
                }
            }
        }

        private void CollectionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateModel();
        }

        private void UpdateAvailableTables()
        {
            using (var dbHandler = new DatabaseHandler(selectedDatabase.DatabasePath))
            {
                Tables.Clear();

                var tables = dbHandler.GetTables();

                foreach (var table in tables)
                {
                    Tables.Add(table.Name);
                }
            }
        }

        private void UpdateAvailableColumns()
        {
            using (var tableHandler = new TableHandler(selectedDatabase.DatabasePath))
            {
                Columns.Clear();

                var columns = tableHandler.GetColumns(selectedTable);

                foreach (var column in columns)
                {
                    Columns.Add(new ColumnItem(column.Name));
                }
            }
        }

        private void UpdateModel()
        {
            VerifyModel();
            UpdateSql();
        }

        private void VerifyModel()
        {
            var selectedColumns = Columns
                .Where(x => !x.SelectedAction.Equals(LocalisationHelper.GetString("IndexCreator_ColumnAction_DoNotUse")));

            IsValidModel = selectedDatabase != null
                && !string.IsNullOrEmpty(indexName)
                && !string.IsNullOrEmpty(selectedTable)
                && selectedColumns.Any();

            if (!IsValidModel)
                StatusInfo = LocalisationHelper.GetString("IndexCreator_StatusInfo_InvalidModel");
            else
                StatusInfo = string.Empty;
        }

        private void UpdateSql()
        {
            var queryBuilder = QueryBuilder.CreateIndex(indexName)
                .IfNotExists(isIfNotExists)
                .On(selectedTable)
                .Unique(isUnique);

            foreach (var item in Columns)
            {
                if (item.SelectedAction.Equals(LocalisationHelper.GetString("IndexCreator_ColumnAction_Ascending")))
                {
                    queryBuilder = queryBuilder.IndexColumn(item.ColumnName, OrderType.Ascending);
                }
                else if(item.SelectedAction.Equals(LocalisationHelper.GetString("IndexCreator_ColumnAction_Descending")))
                {
                    queryBuilder = queryBuilder.IndexColumn(item.ColumnName, OrderType.Descending);
                }
            }

            if (!string.IsNullOrWhiteSpace(whereStatement))
                queryBuilder = queryBuilder.Where(whereStatement);

            SqlStatement = queryBuilder.Build();
        }

        private void Create()
        {
            StatusInfo = string.Empty;

            using (var dbHandler = new DatabaseHandler(selectedDatabase.DatabasePath))
            {
                try
                {
                    dbHandler.ExecuteNonQuery(sqlStatement);
                    StatusInfo = LocalisationHelper.GetString("IndexCreator_StatusInfo_Success");
                    MainTreeHandler.AddIndex(indexName, selectedDatabase.DatabasePath);
                }
                catch (Exception ex)
                {
                    logger.Error("An error occured when the user tried to create an index from the IndexCreator.", ex);
                    StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                }
            }
        }

        private readonly DelegateCommand createCommand;

        public DelegateCommand CreateCommand { get { return createCommand; } }
    }
}
