using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.DBTreeView;

using System;
using System.Collections.Generic;

namespace SQLiteKei.ViewModels.ViewCreatorWindow
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
            set { selectedDatabase = value; ValidateModel(); }
        }

        public List<DatabaseSelectItem> Databases { get; set; }

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
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        private bool isValidViewDefinition;
        public bool IsValidViewDefinition
        {
            get { return isValidViewDefinition; }
            set { isValidViewDefinition = value; NotifyPropertyChanged("IsValidViewDefinition"); }
        }

        public ViewCreatorViewModel()
        {
            IsIfNotExists = true;
            Databases = new List<DatabaseSelectItem>();
            createCommand = new DelegateCommand(Create);
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
                    MainTreeHandler.AddItem<ViewFolderItem, ViewItem>(viewName, selectedDatabase.DatabasePath);
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
