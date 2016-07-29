using SQLiteKei.DataAccess.QueryBuilders.Base;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// A query builder for DROP TRIGGER statements.
    /// </summary>
    public class DropTriggerQueryBuilder : QueryBuilderBase
    {
        private readonly string triggerName;

        private bool ifExists;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropTriggerQueryBuilder"/> class.
        /// </summary>
        /// <param name="triggerName">Name of the trigger.</param>
        public DropTriggerQueryBuilder(string triggerName)
        {
            this.triggerName = triggerName;
        }

        /// <summary>
        /// Defines if the drop statement should include an IF EXISTS or not.
        /// </summary>
        public DropTriggerQueryBuilder IfExists(bool value = true)
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
                return string.Format("DROP TRIGGER IF EXISTS {0}", triggerName);

            return string.Format("DROP TRIGGER {0}", triggerName);
        }
    }
}
