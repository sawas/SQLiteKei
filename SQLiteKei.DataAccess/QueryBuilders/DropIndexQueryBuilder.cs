using SQLiteKei.DataAccess.QueryBuilders.Base;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// A query builder for DROP INDEX statements.
    /// </summary>
    public class DropIndexQueryBuilder : QueryBuilderBase
    {
        private readonly string indexName;

        private bool ifExists;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropIndexQueryBuilder"/> class.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        public DropIndexQueryBuilder(string indexName)
        {
            this.indexName = indexName;
        }

        /// <summary>
        /// Defines if the drop statement should include an IF EXISTS or not.
        /// </summary>
        public DropIndexQueryBuilder IfExists(bool value = true)
        {
            ifExists = value;
            return this;
        }

        /// <summary>
        /// Builds the query.
        /// </summary>
        /// <returns>The query.</returns>
        public override string Build()
        {
            if (ifExists)
                return string.Format("DROP INDEX IF EXISTS '{0}'", indexName);

            return string.Format("DROP INDEX '{0}'", indexName);
        }
    }
}
