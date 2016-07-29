using System.Text;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// A query builder for DROP TABLE statements.
    /// </summary>
    public class DropTableQueryBuilder
    {
        private readonly string table;

        private bool isCascade;

        private bool isIfExists;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropTableQueryBuilder"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public DropTableQueryBuilder(string tableName)
        {
            table = tableName;
        }

        /// <summary>
        /// Defines if the drop statement should include an IF EXISTS or not.
        /// </summary>
        public DropTableQueryBuilder IfExists(bool value = true)
        {
            isIfExists = value;
            return this;
        }

        /// <summary>
        /// Defines if the drop statement should include a CASCADE or not.
        /// </summary>
        /// <returns></returns>
        public DropTableQueryBuilder Cascade()
        {
            isCascade = true;
            return this;
        }

        /// <summary>
        /// Builds the query.
        /// </summary>
        /// <returns>The query.</returns>
        public string Build()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("DROP TABLE");

            if (isIfExists)
                stringBuilder.Append(" IF EXISTS");

            stringBuilder.Append(" '" + table + "'");

            if (isCascade)
                stringBuilder.Append(" CASCADE");

            return stringBuilder.ToString();
        }
    }
}
