using SQLiteKei.DataAccess.QueryBuilders.Base;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// QueryBuilder to generate a statement to create a view.
    /// </summary>
    public class CreateViewQueryBuilder : QueryBuilderBase
    {
        private string viewName;

        private bool isIfNotExists;

        private string asStatement;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateViewQueryBuilder"/> class.
        /// </summary>
        /// <param name="viewName">Name of the new view.</param>
        public CreateViewQueryBuilder(string viewName)
        {
            this.viewName = viewName;
        }

        /// <summary>
        /// Defines if the drop statement should include an IF NOT EXISTS or not.
        /// </summary>
        public CreateViewQueryBuilder IfNotExists(bool value = true)
        {
            isIfNotExists = value;
            return this;
        }

        /// <summary>
        /// Ases the specified as statement.
        /// </summary>
        /// <param name="asStatement">The view defining SQL statement.</param>
        public CreateViewQueryBuilder As(string asStatement)
        {
            this.asStatement = asStatement;
            return this;
        }

        /// <summary>
        /// Builds the query.
        /// </summary>
        /// <returns>The query.</returns>
        public override string Build()
        {
            if (isIfNotExists)
                return string.Format("CREATE VIEW IF NOT EXISTS {0} AS\n{1}", viewName, asStatement);

            return string.Format("CREATE VIEW {0} AS\n{1}", viewName, asStatement);
        }
    }
}
