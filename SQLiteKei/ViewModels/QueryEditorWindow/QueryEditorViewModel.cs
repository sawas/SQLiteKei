using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Data;
using System.Windows.Forms;

namespace SQLiteKei.ViewModels.QueryEditorWindow
{
    public class QueryEditorViewModel : NotifyingModel
    {
        private ILog logger = LogHelper.GetLogger();

        public DatabaseSelectItem SelectedDatabase { get; set; }

        public List<DatabaseSelectItem> Databases { get; set; }

        private string sqlStatement;
        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; NotifyPropertyChanged("SqlStatement"); }
        }

        public string SelectedText { get; set; }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        private ListCollectionView dataGrid;
        public ListCollectionView DataGrid
        {
            get { return dataGrid; }
            set { dataGrid = value; NotifyPropertyChanged("DataGrid"); }
        }

        public QueryEditorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();
            dataGrid = new ListCollectionView(new List<object>());

            executeCommand = new DelegateCommand(Execute);
            saveCommand = new DelegateCommand(SaveQuery);
            loadCommand = new DelegateCommand(LoadQuery);
        }

        private void Execute()
        {
            StatusInfo = string.Empty;

            if (SelectedDatabase == null)
            {
                StatusInfo = LocalisationHelper.GetString("TableCreator_NoDatabaseSelected");
                return;
            }

            if (string.IsNullOrEmpty(SqlStatement)) return;

            if (string.IsNullOrEmpty(SelectedText))
            {
                ExecuteSql(SqlStatement);
            }
            else
            {
                ExecuteSql(SelectedText);
            }
        }

        private void ExecuteSql(string sqlStatement)
        {
            var dbHandler = new DatabaseHandler(SelectedDatabase.DatabasePath);

            try
            {
                if (SqlStatement.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase))
                {
                    var queryResult = dbHandler.ExecuteReader(sqlStatement);

                    DataGrid = new ListCollectionView(queryResult.DefaultView);
                    StatusInfo = string.Format("Rows returned: {0}", queryResult.Rows.Count);
                }
                else
                {
                    var commandResult = dbHandler.ExecuteNonQuery(sqlStatement);

                    StatusInfo = string.Format("Rows affected: {0}", commandResult);
                }
            }
            catch (Exception ex)
            {
                StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
            }
        }

        private DelegateCommand executeCommand;

        public DelegateCommand ExecuteCommand { get { return executeCommand; } }

        private void SaveQuery()
        {
            using (var fileDialog = new SaveFileDialog())
            {
                fileDialog.Filter = "SQL Files(*.sql)|*.sql";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(fileDialog.FileName, sqlStatement);
                        logger.Info("Loaded query to file.");
                    }
                    catch (Exception ex)
                    {
                        logger.Info("Could not save query to file " + fileDialog.FileName, ex);
                        StatusInfo = LocalisationHelper.GetString("QueryEditor_FileSaveFailed");
                    }
                }
            }
        }

        private readonly DelegateCommand saveCommand;

        public DelegateCommand SaveCommand { get { return saveCommand; } }

        private void LoadQuery()
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "SQL Files(*.sql)|*.sql; |All Files |*.*";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        SqlStatement = File.ReadAllText(fileDialog.FileName);
                        logger.Info("Loaded query from file.");
                    }
                    catch (Exception ex)
                    {
                        logger.Info("Could not read query from file " + fileDialog.FileName, ex);
                        StatusInfo = LocalisationHelper.GetString("QueryEditor_FileLoadFailed");
                    }
                }
            }
        }

        private readonly DelegateCommand loadCommand;

        public DelegateCommand LoadCommand { get { return loadCommand; } }
    }
}
