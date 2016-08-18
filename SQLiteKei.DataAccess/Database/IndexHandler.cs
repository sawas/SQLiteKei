using log4net;

using SQLiteKei.DataAccess.Util;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.DataAccess.QueryBuilders;

using System;
using System.Data;
using System.Linq;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class that handles database interactions related to indexes on the specified database
    /// </summary>
    public class IndexHandler : DisposableDbHandler
    {
        private readonly ILog logger = LogHelper.GetLogger();

        public IndexHandler(string databasePath) : base(databasePath)
        {
        }

        /// <summary>
        /// Gets the index with the specified name.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <returns></returns>
        public Index GetIndex(string indexName)
        {
            logger.Info("Loading index " + indexName);
            var indexes = connection.GetSchema("Indexes").AsEnumerable();
            var index = indexes.SingleOrDefault(x => x.ItemArray[5].Equals(indexName));

            return new Index
            {
                Name = index.ItemArray[5].ToString(),
                Table = index.ItemArray[2].ToString(),
                IsUnique = Convert.ToBoolean(index.ItemArray[7]),
                SqlStatement = index.ItemArray[25].ToString()
            };
        }

        /// <summary>
        /// Updates the name of the specified index.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        public void UpdateIndexName(string oldName, string newName)
        {
            using (var command = connection.CreateCommand())
            {
                var dropCommand = QueryBuilder.DropIndex(oldName).Build();
                var index = GetIndex(oldName);
                var createCommand = index.SqlStatement.Replace(oldName, newName);

                command.CommandText = string.Format("BEGIN; {0}; {1}; COMMIT;", dropCommand, createCommand);
                command.ExecuteNonQuery();
                logger.Info("Updated index name from " + oldName + "' to '" + newName + "'.");
            }
        }


        /// <summary>
        /// Updates the uniqueness enforcement of the specified index.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void UpdateIndexUniqueness(string indexName, bool value)
        {
            var index = GetIndex(indexName);
            if (index.IsUnique == value) return;

            using (var command = connection.CreateCommand())
            {
                var dropCommand = QueryBuilder.DropIndex(indexName).Build();

                string newSQL;

                if(value)
                {
                    newSQL = index.SqlStatement.Replace("CREATE INDEX", "CREATE UNIQUE INDEX");
                }
                else
                {
                    newSQL = index.SqlStatement.Replace(" UNIQUE ", " ");
                }

                command.CommandText = string.Format("BEGIN; {0}; {1}; COMMIT;", dropCommand, newSQL);
                command.ExecuteNonQuery();
                logger.Info("Updated index '" + indexName + "'. Set uniqueness enforcement from " + !value + "' to '" + value + "'.");
            }
        }

        /// <summary>
        /// Drops the  specified index from the database.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        public void DropIndex(string indexName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = QueryBuilder.DropIndex(indexName).Build();
                command.ExecuteNonQuery();
                logger.Info("Dropped index '" + indexName + "'.");
            }
        }
    }
}
