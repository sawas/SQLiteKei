using SQLiteKei.DataAccess.Models;
using SQLiteKei.DataAccess.Util.Extensions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class used for calls to the given SQLite database.
    /// </summary>
    public partial class DatabaseHandler : DisposableDbHandler
    {
        public static void CreateDatabase(string filePath)
        {
            SQLiteConnection.CreateFile(filePath);
        }

        public DatabaseHandler(string databasePath) : base(databasePath)
        {
        }

        public DatabaseHandler(DbConnection connection) : base(connection)
        {
        }

        /// <summary>
        /// Returns the name of the current database.
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            return connection.Database;
        }

        /// <summary>
        /// Returns information about all tables in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Table> GetTables()
        {
            var dataRows = GetSchema("Tables");
            var tables = new List<Table>();

            foreach (var row in dataRows)
            {
                tables.Add(new Table
                {
                    Name = row.ItemArray[2].ToString(),
                    CreateStatement = row.ItemArray[6].ToString(),
                });
            }

            return tables;
        }

        /// <summary>
        /// Returns information about all views in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<View> GetViews()
        {
            var dataRows = GetSchema("Views");
            var views = new List<View>();

            foreach (var row in dataRows)
            {
                views.Add(new View
                {
                    Name = row.ItemArray[2].ToString(),
                    SqlStatement = row.ItemArray[3].ToString()
                });
            }

            return views;
        }

        /// <summary>
        /// Returns information about all indexes in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Index> GetIndexes()
        {
            var dataRows = GetSchema("Indexes");
            var indexes = new List<Index>();

            foreach (var row in dataRows)
            {
                var indexName = row.ItemArray[5].ToString();

                if(!indexName.Contains("_PK_"))
                {
                    indexes.Add(new Index
                    {
                        Name = row.ItemArray[5].ToString(),
                        Table = row.ItemArray[2].ToString(),
                        IsUnique = Convert.ToBoolean(row.ItemArray[7]),
                        SqlStatement = row.ItemArray[25].ToString()
                    });
                }
            }

            return indexes;
        }

        /// <summary>
        /// Returns information about all triggers in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Trigger> GetTriggers()
        {
            var dataRows = GetSchema("Triggers");
            var triggers = new List<Trigger>();

            foreach (var row in dataRows)
            {
                triggers.Add(new Trigger
                {
                    Name = row.ItemArray[3].ToString()
                });
            }

            return triggers;
        }

        private IEnumerable<DataRow> GetSchema(string collectionName)
        {
            return connection.GetSchema(collectionName).AsEnumerable();
        }

        /// <summary>
        /// Gets the database settings. Settings that could not be loaded will be null.
        /// </summary>
        /// <returns>The database settings.</returns>
        public DbSettings GetSettings()
        {
            var settings = new DbSettings
            {
                SchemaVersion = Pragma("Schema_Version").ConvertTo<short?>(),
                UserVersion = Pragma("User_Version").ConvertTo<short?>(),
                ApplicationId = Pragma("Application_Id").ConvertTo<int?>(),
                PageSize = Pragma("Page_Size").ConvertTo<int?>(),
                MaxPageCount = Pragma("Max_Page_Count").ConvertTo<int?>(),
                PageCount = Pragma("Page_Count").ConvertTo<int?>(),
                FreeListCount = Pragma("Freelist_Count").ConvertTo<int?>(),
                JournalMode = Pragma("Journal_Mode").ToString(),
                JournalSizeLimit = Pragma("Journal_Size_Limit").ConvertTo<int?>(),
                CacheSize = Pragma("Cache_Size").ConvertTo<int?>()
            };
            
            return settings;
        }

        private object Pragma(string pragmaName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"PRAGMA {pragmaName};";
                var resultTable = new DataTable();
                resultTable.Load(command.ExecuteReader());

                return resultTable.Rows[0].ItemArray[0];
            }
        }

        /// <summary>
        /// Updates the database settings with the given parameters.
        /// </summary>
        /// <param name="newSettings">The new settings.</param>
        public void UpdateSettings(DbSettings newSettings)
        {
            var currentSettings = GetSettings();

            UpdateSetting("Schema_Version", currentSettings.SchemaVersion, newSettings.SchemaVersion);
            UpdateSetting("User_Version", currentSettings.UserVersion, newSettings.UserVersion);
            UpdateSetting("Application_Id", currentSettings.ApplicationId, newSettings.ApplicationId);
            UpdateSetting("Page_Size", currentSettings.PageSize, newSettings.PageSize);
            UpdateSetting("Max_Page_Count", currentSettings.MaxPageCount, newSettings.MaxPageCount);
            UpdateSetting("Journal_Mode", currentSettings.JournalMode, newSettings.JournalMode);
            UpdateSetting("Journal_Size_Limit", currentSettings.JournalSizeLimit, newSettings.JournalSizeLimit);
            UpdateSetting("Cache_Size", currentSettings.CacheSize, newSettings.CacheSize);
        }

        private void UpdateSetting(string pragmaName, object currentValue, object newValue)
        {
            if (!currentValue.Equals(newValue))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"PRAGMA {pragmaName}={newValue}";
                    var result= command.ExecuteNonQuery();
                }
            }
        }
    }
}