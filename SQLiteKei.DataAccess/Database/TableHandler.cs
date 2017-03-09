using log4net;

using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.Util;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.DataAccess.QueryBuilders;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Data.Common;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class that handles database interactions related to tables on the specified database
    /// </summary>
    public class TableHandler : DisposableDbHandler
    {
        public TableHandler(string databasePath) : base(databasePath)
        {
        }

        public TableHandler(DbConnection connection) : base(connection)
        {
        }

        /// <summary>
        /// Gets the table with the specified name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
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
                        DefaultValue = row.ItemArray[4].ToString(),
                        IsPrimary = Convert.ToBoolean(row.ItemArray[5])
                    });
                }
            }
            return columns;
        }

        /// <summary>
        /// Returns a datatable with all rows of the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public DataRowCollection GetRows(string tableName)
        {
            logger.Info("Loading rows for table '" + tableName + "'.");

            using (var command = connection.CreateCommand())
            {
                command.CommandText = QueryBuilder.Select().From(tableName).Build();

                var resultTable = new DataTable();
                resultTable.Load(command.ExecuteReader());

                return resultTable.Rows;
            }
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
        public void Drop(string tableName)
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
                command.CommandText = $"REINDEX '{tableName}'";
                command.ExecuteNonQuery();
                logger.Info("Reindexed table '" + tableName + "'.");
            }
        }

        public void AddColumn(string tableName, string columnName, string dataType, bool isNullable = false, string defaultValue = null)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = QueryBuilder.AlterTable(tableName)
                    .AddColumn(columnName, dataType, isNullable, defaultValue)
                    .Build();
                command.ExecuteNonQuery();
                logger.Info("Added column '" + columnName + "' on table '" + tableName + "'.");
            }
        }

        /// <summary>
        /// Renames the specified column.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="tableName">Name of the table on which the column was defined.</param>
        public void RenameColumn(string oldName, string newName, string tableName)
        {
            var originalColumns = GetColumns(tableName);

            var tmpQueryBuilder = QueryBuilder.CreateTable("SQLiteKei_TMP1").AsTemporary();
            var originalQueryBuilder = QueryBuilder.CreateTable(tableName);
            var newColumns = new List<string>();
            var oldColumns = new List<string>();

            foreach (var column in originalColumns)
            {
                if(column.Name.Equals(oldName))
                {
                    tmpQueryBuilder.AddColumn(newName, column.DataType, column.IsPrimary, column.IsNotNullable, column.DefaultValue);
                    originalQueryBuilder.AddColumn(newName, column.DataType, column.IsPrimary, column.IsNotNullable, column.DefaultValue);
                    newColumns.Add(newName);
                    oldColumns.Add(column.Name);
                }
                else
                {
                    tmpQueryBuilder.AddColumn(column.Name, column.DataType, column.IsPrimary, column.IsNotNullable, column.DefaultValue);
                    originalQueryBuilder.AddColumn(column.Name, column.DataType, column.IsPrimary, column.IsNotNullable, column.DefaultValue);
                    newColumns.Add(column.Name);
                    oldColumns.Add(column.Name);
                }
            }

            using (var command = connection.CreateCommand())
            {
                var combinedOldColumns = string.Join(",", oldColumns);
                var combinedNewColumns = string.Join(",", newColumns);

                command.CommandText = "BEGIN TRANSACTION;\n"
                    + tmpQueryBuilder.Build() + "\n"
                    + $"INSERT INTO SQLiteKei_TMP1 SELECT {combinedOldColumns} FROM '{tableName}';\n"
                    + QueryBuilder.DropTable(tableName).Build() + ";\n"
                    + originalQueryBuilder.Build() + "\n"
                    + $"INSERT INTO {tableName} SELECT {combinedNewColumns} FROM 'SQLiteKei_TMP1';\n"
                    + QueryBuilder.DropTable("SQLiteKei_TMP1").Build() + ";\n"
                    + "COMMIT;";

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes the specified column. Note: All Foreign Key constraints on the table will be lost.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        public void DeleteColumn(string tableName, string columnName)
        {
            var columns = GetColumns(tableName);

            var tmpQueryBuilder = QueryBuilder.CreateTable("SQLiteKei_TMP1").AsTemporary();
            var originalQueryBuilder = QueryBuilder.CreateTable(tableName);
            var columnsToPreserve = new List<string>();

            foreach(var column in columns)
            {
                if(!column.Name.Equals(columnName))
                {
                    tmpQueryBuilder.AddColumn(column.Name, column.DataType, column.IsPrimary, column.IsNotNullable, column.DefaultValue);
                    originalQueryBuilder.AddColumn(column.Name, column.DataType, column.IsPrimary, column.IsNotNullable, column.DefaultValue);
                    columnsToPreserve.Add(column.Name);
                }
            }
            
            using (var command = connection.CreateCommand())
            {
                var combinedColumnsToPreserve = string.Join(",", columnsToPreserve);

                command.CommandText = "BEGIN TRANSACTION;\n"
                    + tmpQueryBuilder.Build() + "\n"
                    + $"INSERT INTO SQLiteKei_TMP1 SELECT {combinedColumnsToPreserve} FROM '{tableName}';\n"
                    + QueryBuilder.DropTable(tableName).Build() + ";\n"
                    + originalQueryBuilder.Build() + "\n"
                    + $"INSERT INTO {tableName} SELECT {combinedColumnsToPreserve} FROM 'SQLiteKei_TMP1';\n"
                    + QueryBuilder.DropTable("SQLiteKei_TMP1").Build() + ";\n"
                    + "COMMIT;";

                command.ExecuteNonQuery();
            }
        }
    }
}
