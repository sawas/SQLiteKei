using log4net;

using SQLiteKei.DataAccess.Helpers;
using SQLiteKei.DataAccess.QueryBuilders;

namespace SQLiteKei.DataAccess.Database
{
    public class IndexHandler : DisposableDbHandler
    {
        private readonly ILog logger = LogHelper.GetLogger();

        public IndexHandler(string databasePath) : base(databasePath)
        {
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
