using log4net;

using SQLiteKei.DataAccess.Helpers;
using SQLiteKei.DataAccess.QueryBuilders;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class that handles database interactions related to triggers on the specified database
    /// </summary>
    public class TriggerHandler : DisposableDbHandler
    {
        private ILog logger = LogHelper.GetLogger();

        public TriggerHandler(string databasePath) : base(databasePath)
        {
        }

        public void DropTrigger(string triggerName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = QueryBuilder.DropTrigger(triggerName).Build();
                command.ExecuteNonQuery();
                logger.Info("Dropped trigger '" + triggerName + "'.");
            }
        }
    }
}
