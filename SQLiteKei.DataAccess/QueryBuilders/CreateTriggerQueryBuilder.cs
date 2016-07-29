using System.Collections.Generic;
using System.Text;

using SQLiteKei.DataAccess.QueryBuilders.Base;
using System;
using System.Linq;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// QueryBuilder to generate a statement to create a trigger.
    /// </summary>
    public class CreateTriggerQueryBuilder : QueryBuilderBase
    {
        private readonly string triggerName;

        private bool isIfNotExists;

        private string triggerEntryPoint;

        private string triggerEvent;

        private string targetTableName;

        private bool isForEachRow;

        private string condition;

        private string triggerActions;

        public CreateTriggerQueryBuilder(string triggerName)
        {
            this.triggerName = triggerName;
        }

        /// <summary>
        /// Defines if the statement should include an IF NOT EXISTS or not.
        /// </summary>
        public CreateTriggerQueryBuilder IfNotExists(bool value = true)
        {
            isIfNotExists = value;
            return this;
        }

        /// <summary>
        /// Sets the entry point of the trigger to BEFORE.
        /// </summary>
        public CreateTriggerQueryBuilder Before()
        {
            triggerEntryPoint = "BEFORE";
            return this;
        }

        /// <summary>
        /// Sets the entry point of the trigger to AFTER.
        /// </summary>
        public CreateTriggerQueryBuilder After()
        {
            triggerEntryPoint = "AFTER";
            return this;
        }

        /// <summary>
        /// Sets the entry point of the trigger to INSTEAD OF.
        /// </summary>
        public CreateTriggerQueryBuilder InsteadOf()
        {
            triggerEntryPoint = "INSTEAD OF";
            return this;
        }

        /// <summary>
        /// Sets the trigger raising event to INSERT.
        /// </summary>
        public CreateTriggerQueryBuilder Insert()
        {
            triggerEvent = "INSERT";
            return this;
        }

        /// <summary>
        /// Sets the trigger raising event to UDATE.
        /// </summary>
        public CreateTriggerQueryBuilder Update()
        {
            triggerEvent = "UPDATE";
            return this;
        }

        /// <summary>
        /// Sets the trigger raising event to UPDATE OF with the specified columns.
        /// </summary>
        public CreateTriggerQueryBuilder Update(List<string> columnNames)
        {
            if (!columnNames.Any())
                triggerEvent = "UPDATE OF " + string.Join(", ", columnNames);
            else
                triggerEvent = "UPDATE";
            return this;
        }

        /// <summary>
        /// Sets the trigger raising event to DELETE.
        /// </summary>
        public CreateTriggerQueryBuilder Delete()
        {
            triggerEvent = "DELETE";
            return this;
        }

        /// <summary>
        /// Specifies the table on which the trigger will be targeting.
        /// </summary>
        /// <param name="targetTableName">Name of the target table.</param>
        /// <returns></returns>
        public CreateTriggerQueryBuilder On(string targetTableName)
        {
            this.targetTableName = targetTableName;
            return this;
        }

        /// <summary>
        /// Specifies if the trigger will include FOR EACH ROW.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public CreateTriggerQueryBuilder ForEachRow(bool value = true)
        {
            isForEachRow = value;
            return this;
        }

        /// <summary>
        /// Specifies an additional condition on which the trigger will be raised.
        /// </summary>
        /// <param name="condition">The condition.</param>
        public CreateTriggerQueryBuilder When(string condition)
        {
            this.condition = condition;
            return this;
        }

        /// <summary>
        /// Specifies the actual trigger action(s).
        /// </summary>
        /// <param name="triggerActions">The trigger actions.</param>
        public CreateTriggerQueryBuilder Do(string triggerActions)
        {
            this.triggerActions = triggerActions;
            return this;
        }

        /// <summary>
        /// Builds the query.
        /// </summary>
        /// <returns>The query.</returns>
        public override string Build()
        {
            var stringBuilder = new StringBuilder("CREATE TRIGGER");

            if (isIfNotExists)
                stringBuilder.Append(" IF NOT EXISTS");

            stringBuilder.Append(" '" + triggerName + "'");
            stringBuilder.Append("\n" + triggerEntryPoint);
            stringBuilder.Append(" " + triggerEvent);
            stringBuilder.Append(" ON '" + targetTableName + "'");

            if (isForEachRow)
                stringBuilder.Append("\nFOR REACH ROW");

            if (!string.IsNullOrWhiteSpace(condition))
                stringBuilder.Append("\nWHEN " + condition);

            stringBuilder.Append("\nBEGIN\n" + triggerActions + "\nEND");

            return stringBuilder.ToString();
        }
    }
}
