using log4net;

using SQLiteKei.DataAccess.Helpers;
using SQLiteKei.ViewModels.Common;

using System.Collections.Generic;

namespace SQLiteKei.ViewModels.CreatorWindows.IndexCreator
{
    public class IndexCreatorViewModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private DatabaseSelectItem selectedDatabase;
        public DatabaseSelectItem SelectedDatabase
        {
            get { return selectedDatabase; }
            set { selectedDatabase = value; }
        }

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

        public List<DatabaseSelectItem> Databases { get; set; }

        public IndexCreatorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();

            
        }
    }
}
