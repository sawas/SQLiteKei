using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.Properties;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;

using System;
using System.ComponentModel;
using System.Windows.Data;

namespace SQLiteKei.ViewModels.MainTabControl.Views
{
    public class GeneralViewViewModel : NotifyingModel
    {
        private ILog logger = LogHelper.GetLogger();

        private string viewName;
        public string ViewName
        {
            get { return viewName; }
            set
            { 
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        using (var viewHandler = new ViewHandler(Settings.Default.CurrentDatabase))
                        {
                            viewHandler.UpdateViewName(viewName, value);
                            MainTreeHandler.UpdateViewName(viewName, value, Settings.Default.CurrentDatabase);
                            viewName = value;
                        }
                    }
                    catch(Exception ex)
                    {
                        logger.Warn("Failed to rename view '" + viewName + "' from table overview.", ex);
                    }
                }
            }
        }

        private string sqlStatement;
        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; }
        }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        private ICollectionView dataGridCollection;
        public ICollectionView DataGridCollection
        {
            get { return dataGridCollection; }
            set { dataGridCollection = value; NotifyPropertyChanged("DataGridCollection"); }
        }

        private bool couldLoadResults;
        public bool CouldLoadResults
        {
            get { return couldLoadResults; }
            set { couldLoadResults = value; NotifyPropertyChanged("CouldLoadResults"); }
        }

        private bool couldNotLoadResults;
        public bool CouldNotLoadResults
        {
            get { return couldNotLoadResults; }
            set { couldNotLoadResults = value; NotifyPropertyChanged("CouldNotLoadResults"); }
        }

        public GeneralViewViewModel(string viewName)
        {
            this.viewName = viewName;
            updateCommand = new DelegateCommand(UpdateViewDefinition);
            Initialize();
        }

        private void Initialize()
        {
            using (var viewHandler = new ViewHandler(Settings.Default.CurrentDatabase))
            {
                SqlStatement = viewHandler.GetViewDefinition(viewName);
            }

            SetDataGridData();
        }

        private void SetDataGridData()
        {
            try
            {
                using (var dbHandler = new DatabaseHandler(Settings.Default.CurrentDatabase))
                {
                    var resultTable = dbHandler.ExecuteReader(SqlStatement);
                    DataGridCollection = new ListCollectionView(resultTable.DefaultView);
                    CouldNotLoadResults = false;
                    CouldLoadResults = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Error("The data of a view could not be loaded.", ex);
                StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");

                CouldNotLoadResults = true;
                CouldLoadResults = false;
            }
        }

        private void UpdateViewDefinition()
        {
            StatusInfo = string.Empty;

            if (!SqlStatement.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase))
            {
                StatusInfo = LocalisationHelper.GetString("ViewGeneralTab_InvalidStatement");
                return;
            } 

            try
            {
                using (var viewHandler = new ViewHandler(Settings.Default.CurrentDatabase))
                {
                    viewHandler.UpdateViewDefinition(viewName, SqlStatement);
                }
            }
            catch(Exception ex)
            {
                logger.Error("A view definition could not be updated.", ex);
                StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
            }

            SetDataGridData();
        }

        private readonly DelegateCommand updateCommand;

        public DelegateCommand UpdateCommand { get { return updateCommand; } }
    }
}
