using SQLiteKei.DataAccess.QueryBuilders.Base;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// A query builder for DROP VIEW statements.
    /// </summary>
    public class DropViewQueryBuilder : QueryBuilderBase
    {
        private readonly string viewName;

        private bool ifExists;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropViewQueryBuilder"/> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        public DropViewQueryBuilder(string viewName)
        {
            this.viewName = viewName;
        }

        /// <summary>
        /// Defines if the drop statement should include an IF EXISTS or not.
        /// </summary>
        public DropViewQueryBuilder IfExists(bool value = true)
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
                return string.Format("DROP VIEW IF EXISTS '{0}'", viewName);

            return string.Format("DROP VIEW '{0}'", viewName);
        }
    }
}
