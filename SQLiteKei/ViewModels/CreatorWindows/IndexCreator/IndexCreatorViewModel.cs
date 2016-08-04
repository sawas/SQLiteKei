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

        public List<DatabaseSelectItem> Databases { get; set; }

        public IndexCreatorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();

            
        }
    }
}
