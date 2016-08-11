using log4net;

using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.Helpers;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.DataAccess.QueryBuilders;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class that handles database interactions related to tables on the specified database
    /// </summary>
    public class TableHandler : DisposableDbHandler
    {
        private ILog logger = LogHelper.GetLogger();

        public TableHandler(string databasePath) : base(databasePath)
        {
        }

        public Table GetTable(string tableName)
        {
            logger.Info("Loading table '" + tableName + "'.");
            var tables = connection.GetSchema("Tables").AsEnumerable();
            var table = tables.SingleOrDefault(x => x.ItemArray[2].Equals(tableName));

            return new Table
            {
                Name = table.ItemArray[2].ToString(),
                CreateStatement = table.ItemArray[6].ToString()
            };
        }

        /// <summary>
        /// Gets the column meta data for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public List<Column> GetColumns(string tableName)
        {
            var columns = new List<Column>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA table_info('" + tableName + "');";

                var resultTable = new DataTable();
                resultTable.Load(command.ExecuteReader());

                foreach (DataRow row in resultTable.Rows)
                {
                    columns.Add(new Column
                    {
                        Id = Convert.ToInt32(row.ItemArray[0]),
                        Name = (string)row.ItemArray[1],
                        DataType = (string)row.ItemArray[2],
                        IsNotNullable = Convert.ToBoolean(row.ItemArray[3]),
                        DefaultValue = row.ItemArray[4],
                        IsPrimary = Convert.ToBoolean(row.ItemArray[5])
                    });
                }
            }
            return columns;
        }

        /// <summary>
        /// Gets the row count for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public long GetRowCount(string tableName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = QueryBuilder
                .Select("count(*)")
                .From(tableName)
                .Build();

                return Convert.ToInt64(command.ExecuteScalar());
            }
        }

        /// <summary>
        /// Drops the specified table from the given database. Sends a plain command to the database without any further error handling.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public void DropTable(string tableName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = QueryBuilder.DropTable(tableName).Build();
                command.ExecuteNonQuery();
                logger.Info("Dropped table '" + tableName + "'.");
            }
        }

        /// <summary>
        /// Renames the specified table.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        public void RenameTable(string oldName, string newName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "ALTER TABLE '" + oldName + "' RENAME TO '" + newName + "'";
                command.ExecuteNonQuery();
                logger.Info("Renamed table '" + oldName + "' to '" + newName + "'.");
            }
        }

        /// <summary>
        /// Deletes all rows from the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public void EmptyTable(string tableName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("DELETE FROM {0}", tableName);

                try
                {
                    command.ExecuteNonQuery();
                    logger.Info("Emptied table '" + tableName + "'.");
                }
                catch(SQLiteException ex)
                {
                    if(ex.Message.Contains("no such table"))
                    {
                        logger.Info("Could not empty table '" + tableName + "'. No such table found.");
                        throw new TableNotFoundException(tableName);
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// Reindexes the specified table.
        /// </summary>
        /// <param name="tableName"></param>
        public void ReindexTable(string tableName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("REINDEX '{0}'", tableName);
                command.ExecuteNonQuery();
                logger.Info("Reindexed table '" + tableName + "'.");
            }
        }

        public void DeleteColumn(string tableName, string columnName)
        {
            var columns = GetColumns(tableName);

            var queryBuilder = QueryBuilder.CreateTable("SQLiteKei_TMP1");

            foreach(var column in columns)
            {
                //queryBuilder.AddColumn(column.Name, )
            }

            using (var command = connection.CreateCommand())
            {
                var asdasd = "BEGIN TRANSACTION"
                    + "CREATE TEMPORARY TABLE SQLiteKei_TMP1"
                    + "INSERT INTO ";
                command.CommandText = string.Format("BEGIN TRANSACTION"
                    );
            }
        }
    }
}
