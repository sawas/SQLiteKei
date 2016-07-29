namespace SQLiteKei.DataAccess.QueryBuilders.Base
{
    /// <summary>
    /// The base class for QueryBuilders.
    /// </summary>
    public abstract class QueryBuilderBase
    {
        /// <summary>
        /// Builds the query.
        /// </summary>
        /// <returns>The query.</returns>
        public abstract string Build();
    }
}
