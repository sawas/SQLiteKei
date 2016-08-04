namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// Allows to build SQL query strings.
    /// </summary>
    public abstract class QueryBuilder
    {
        /// <summary>
        /// Begins a SELECT statement. If no further columns are defined, the statement will be created using a wildcard.
        /// </summary>
        public static SelectQueryBuilder Select()
        {
            return new SelectQueryBuilder();
        }

        /// <summary>
        /// Begins a SELECT statement.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public static SelectQueryBuilder Select(string column)
        {
            return new SelectQueryBuilder(column);
        }

        /// <summary>
        /// Begins a SELECT statement. If no columns are defined, it defaults to a wildcard selection.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public static SelectQueryBuilder Select(string column, string alias)
        {
            return new SelectQueryBuilder(column, alias);
        }

        /// <summary>
        /// Begins a CREATE statement for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static CreateTableQueryBuilder CreateTable(string tableName)
        {
            return new CreateTableQueryBuilder(tableName);
        }

        /// <summary>
        /// Begins a CREATE statement for the specified view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        public static CreateViewQueryBuilder CreateView(string viewName)
        {
            return new CreateViewQueryBuilder(viewName);
        }

        /// <summary>
        /// Begins a CREATE statement for the specified trigger.
        /// </summary>
        /// <param name="triggerName">Name of the trigger.</param>
        /// <returns></returns>
        public static CreateTriggerQueryBuilder CreateTrigger(string triggerName)
        {
            return new CreateTriggerQueryBuilder(triggerName);
        }

        /// <summary>
        /// Begins a DROP statement for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static DropTableQueryBuilder DropTable(string tableName)
        {
            return new DropTableQueryBuilder(tableName);
        }

        /// <summary>
        /// Begins a DROP statement for the specified view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        public static DropViewQueryBuilder DropView(string viewName)
        {
            return new DropViewQueryBuilder(viewName);
        }

        /// <summary>
        /// Begins a DROP statement for the specified index.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        public static DropIndexQueryBuilder DropIndex(string indexName)
        {
            return new DropIndexQueryBuilder(indexName);
        }

        /// <summary>
        /// Begins a DROP statement for the specified trigger.
        /// </summary>
        /// <param name="triggerName">Name of the trigger.</param>
        /// <returns></returns>
        public static DropTriggerQueryBuilder DropTrigger(string triggerName)
        {
            return new DropTriggerQueryBuilder(triggerName);
        }
    }
}
