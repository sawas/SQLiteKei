using log4net;

using SQLiteKei.DataAccess.Util;

using System;
using System.Data.Common;
using System.Data.SQLite;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// The base class for DbHandlers which implements IDisposable and provides a DbConnection for the specified sqlite database.
    /// </summary>
    public abstract class DisposableDbHandler : IDisposable
    {
        protected ILog logger = LogHelper.GetLogger();

        protected SQLiteConnection connection;

        protected DisposableDbHandler(string databasePath)
        {
            InitializeConnection(databasePath);
        }

        protected DisposableDbHandler(DbConnection connection)
        {
            this.connection = connection as SQLiteConnection;
            connection.Open();
        }

        private void InitializeConnection(string databasePath)
        {
            try
            {
                connection = new SQLiteConnection(databasePath)
                {
                    ConnectionString = string.Format("Data Source={0};Pooling=true", databasePath)
                };

                connection.Open();
            }
            catch (Exception ex)
            {
                logger.Error("Could not initialize connection to SQLite database.", ex);
            }
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
