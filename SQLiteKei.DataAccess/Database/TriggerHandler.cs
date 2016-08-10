using log4net;

using SQLiteKei.DataAccess.Helpers;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.DataAccess.QueryBuilders;

using System.Data;
using System.Linq;

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

        /// <summary>
        /// Gets the trigger with the specified name.
        /// </summary>
        /// <param name="triggerName">Name of the trigger.</param>
        /// <returns></returns>
        public Trigger GetTrigger(string triggerName)
        {
            logger.Info("Loading trigger " + triggerName);
            var triggers = connection.GetSchema("Triggers").AsEnumerable();
            var trigger = triggers.SingleOrDefault(x => x.ItemArray[3].Equals(triggerName));

            return new Trigger
            {
                Name = trigger.ItemArray[3].ToString(),
                Target = trigger.ItemArray[2].ToString(),
                SqlStatement = trigger.ItemArray[4].ToString()
            };
        }

        /// <summary>
        /// Updates the name of the specified trigger.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        public void UpdateTriggerName(string oldName, string newName)
        {
            using (var command = connection.CreateCommand())
            {
                var dropCommand = QueryBuilder.DropTrigger(oldName).Build();
                var trigger = GetTrigger(oldName);
                var newSQL = trigger.SqlStatement.Replace(oldName, newName);

                command.CommandText = string.Format("BEGIN; {0}; {1}; COMMIT;", dropCommand, newSQL);
                command.ExecuteNonQuery();
                logger.Info("Updated trigger name from " + oldName + "' to '" + newName + "'.");
            }
        }

        /// <summary>
        /// Drops the specified trigger.
        /// </summary>
        /// <param name="triggerName">Name of the trigger.</param>
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
