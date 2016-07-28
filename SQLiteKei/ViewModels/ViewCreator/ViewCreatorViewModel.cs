using log4net;

using SQLiteKei.Commands;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;
using SQLiteKei.DataAccess.Helpers;

using System.Collections.Generic;

namespace SQLiteKei.ViewModels.ViewCreator
{
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

        public bool IfIsNotExists { get; set; }

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
            IfIsNotExists = true;
            Databases = new List<DatabaseSelectItem>();
            createCommand = new DelegateCommand(Create);
        }

        private void Create()
        {
            StatusInfo = string.Empty;
        }

        public void ValidateModel()
        {
            IsValidViewDefinition = SelectedDatabase != null
                && !string.IsNullOrWhiteSpace(ViewName)
                && !string.IsNullOrWhiteSpace(SqlStatement);
        }

        private readonly DelegateCommand createCommand;

        public DelegateCommand CreateCommand { get { return createCommand; } }
    }
}
