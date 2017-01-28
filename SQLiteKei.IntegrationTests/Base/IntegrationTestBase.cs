using NUnit.Framework;
using System.Data.SQLite;
using System.IO;

namespace SQLiteKei.IntegrationTests.Base
{
    public class IntegrationTestBase
    {
        protected string testDatabaseFile;

        private string connectionString;

        protected SQLiteConnection connection;

        [OneTimeSetUp]
        public void CreateDatabase()
        {
            testDatabaseFile = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\TestDb");

            if(!File.Exists(testDatabaseFile))
            {
                SQLiteConnection.CreateFile(testDatabaseFile);
            }

            connectionString = string.Format("Data Source={0}", testDatabaseFile);
        }

        [SetUp]
        public void ResetFakeData()
        {
            var oneTimeConnection = new SQLiteConnection();
            oneTimeConnection.ConnectionString = connectionString;
            oneTimeConnection.Open();

            for (var i = 1; i <= 10; i++)
            {
                using (var dropCommand = oneTimeConnection.CreateCommand())
                {
                    dropCommand.CommandText = string.Format("DROP TABLE IF EXISTS TEST{0}", i);
                    dropCommand.ExecuteNonQuery();
                }

                using (var createCommand = oneTimeConnection.CreateCommand())
                {
                    createCommand.CommandText = string.Format("CREATE TABLE TEST{0} (ColumnA{0} varchar({1}), ColumnB{0} int)", i, i + 50);
                    createCommand.ExecuteNonQuery();
                }                    

                for (var j = 1; j <= i; j++)
                {

                    using (var insertCommand = oneTimeConnection.CreateCommand())
                    {
                        insertCommand.CommandText = string.Format("INSERT INTO TEST{0} VALUES('ENTRY{1}', '{1}')", i, j);
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }

            for (var i = 1; i <= 5; i++)
            {
                using (var createCommand = oneTimeConnection.CreateCommand())
                {
                    createCommand.CommandText = string.Format("CREATE VIEW IF NOT EXISTS View{0} AS SELECT * FROM TEST{0} ", i);
                    createCommand.ExecuteNonQuery();
                }
            }
            oneTimeConnection.Close();
        }

        [SetUp]
        public void EstablishConnection()
        {
            connection = new SQLiteConnection(connectionString);
        }
    }
}
