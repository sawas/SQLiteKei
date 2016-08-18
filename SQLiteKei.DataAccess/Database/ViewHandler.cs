using log4net;

using SQLiteKei.DataAccess.Util;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.DataAccess.QueryBuilders;

using System.Data;
using System.Linq;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class that handles database interactions related to views on the specified database.
    /// </summary>
    public class ViewHandler : DisposableDbHandler
    {
        private ILog logger = LogHelper.GetLogger();

        public ViewHandler(string databasePath) : base(databasePath)
        {
        }

        /// <summary>
        /// Updates the name of the specified view.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        public void UpdateViewName(string oldName, string newName)
        {
            using (var command = connection.CreateCommand())
            {
                var dropCommand = QueryBuilder.DropView(oldName).Build();
                var view = GetView(oldName);
                var createCommand = QueryBuilder.CreateView(newName).As(view.SqlStatement).Build();

                command.CommandText = string.Format("BEGIN; {0}; {1}; COMMIT;", dropCommand, createCommand);
                command.ExecuteNonQuery();
                logger.Info("Updated view name from" + oldName + "' to '" + newName + "'.");
            }
        }

        /// <summary>
        /// Updates the SQL statement of the specified view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="newSql">The new SQL statement.</param>
        public void UpdateViewDefinition(string viewName, string newSql)
        {
            using (var command = connection.CreateCommand())
            {
                var dropCommand = QueryBuilder.DropView(viewName).Build();
                var createCommand = QueryBuilder.CreateView(viewName).As(newSql).Build();

                command.CommandText = string.Format("BEGIN; {0}; {1}; COMMIT;", dropCommand, createCommand);
                command.ExecuteNonQuery();
                logger.Info("Updated SQL statement of view " + viewName + "' to '" + newSql + "'.");
            }
        }

        /// <summary>
        /// Gets the specified view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        public View GetView(string viewName)
        {
            logger.Info("Loading view " + viewName);
            var views = connection.GetSchema("Views").AsEnumerable();
            var view = views.SingleOrDefault(x => x.ItemArray[2].Equals(viewName));

            return new View
            {
                Name = view.ItemArray[2].ToString(),
                SqlStatement = view.ItemArray[3].ToString()
            };
        }

        public string GetViewDefinition(string viewName)
        {
            logger.Info("Loading view definition of view " + viewName);
            var dataRows = connection.GetSchema("Views").AsEnumerable();
            var selectedRow = dataRows.SingleOrDefault(x => x.ItemArray[2].Equals(viewName));

            return selectedRow.ItemArray[3].ToString();
        }

        /// <summary>
        /// Drops the specified view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
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
