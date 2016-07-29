using log4net;

using SQLiteKei.DataAccess.Helpers;
using SQLiteKei.DataAccess.QueryBuilders;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class that handles database interactions related to views on the specified database
    /// </summary>
    public class ViewHandler : DisposableDbHandler
    {
        private ILog logger = LogHelper.GetLogger();

        public ViewHandler(string databasePath) : base(databasePath)
        {
        }

        public void DropView(string viewName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = QueryBuilder.DropView(viewName).Build();
                command.ExecuteNonQuery();
                logger.Info("Dropped view '" + viewName + "'.");
            }
        }
    }
}
