using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Helpers;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;

using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public List<DatabaseSelectItem> Databases { get; set; }

        private string indexName;
        public string IndexName
        {
            get { return indexName; }
            set { indexName = value; }
        }

        private bool isIfNotExists;
        public bool IsIfNotExists
        {
            get { return isIfNotExists; }
            set { isIfNotExists = value; }
        }

        private string selectedTable;
        public string SelectedTable
        {
            get { return selectedTable; }
            set { selectedTable = value; UpdateAvailableColumns(); }
        }

        public ObservableCollection<string> Tables { get; set; }

        public bool IsUnique { get; set; }

        public ObservableCollection<ColumnItem> Columns { get; set; }

        private string whereStatement;
        public string WhereStatement
        {
            get { return whereStatement; }
            set { whereStatement = value; }
        }

        public IndexCreatorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();
            Tables = new ObservableCollection<string>();
            Columns = new ObservableCollection<ColumnItem>();

            createCommand = new DelegateCommand(Create);
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

        private void Create()
        {

        }

        private readonly DelegateCommand createCommand;

        public DelegateCommand CreateCommand { get { return createCommand; } }
    }
}
