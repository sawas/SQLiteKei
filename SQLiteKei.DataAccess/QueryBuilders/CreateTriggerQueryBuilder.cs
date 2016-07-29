using System.Collections.Generic;
using System.Text;

using SQLiteKei.DataAccess.QueryBuilders.Base;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// QueryBuilder to generate a statement to create a trigger.
    /// </summary>
    public class CreateTriggerQueryBuilder : QueryBuilderBase
    {
        private readonly string triggerName;

        private bool isIfNotExists;

        private string triggerOccurence;

        private string triggerEvent;

        private string targetTableName;

        private bool isForEachRow;

        private string condition;

        private string triggerActions;

        public CreateTriggerQueryBuilder(string triggerName)
        {
            this.triggerName = triggerName;
        }

        public CreateTriggerQueryBuilder IfNotExists(bool value = true)
        {
            isIfNotExists = value;
            return this;
        }

        public CreateTriggerQueryBuilder Before()
        {
            triggerOccurence = "BEFORE";
            return this;
        }

        public CreateTriggerQueryBuilder After()
        {
            triggerOccurence = "AFTER";
            return this;
        }

        public CreateTriggerQueryBuilder InsteadOf()
        {
            triggerOccurence = "INSTEAD OF";
            return this;
        }

        public CreateTriggerQueryBuilder Insert()
        {
            triggerEvent = "INSERT";
            return this;
        }

        public CreateTriggerQueryBuilder Update()
        {
            triggerEvent = "UPDATE";
            return this;
        }

        public CreateTriggerQueryBuilder Update(List<string> columnNames)
        {
            triggerOccurence = "UPDATE OF " + string.Join(", ", columnNames);
            return this;
        }

        public CreateTriggerQueryBuilder Delete()
        {
            triggerEvent = "DELETE";
            return this;
        }

        public CreateTriggerQueryBuilder On(string targetTableName)
        {
            this.targetTableName = targetTableName;
            return this;
        }

        public CreateTriggerQueryBuilder ForEachRow(bool value = true)
        {
            isForEachRow = value;
            return this;
        }

        public CreateTriggerQueryBuilder When(string condition)
        {
            this.condition = condition;
            return this;
        }

        public CreateTriggerQueryBuilder Do(string triggerActions)
        {
            this.triggerActions = triggerActions;
            return this;
        }

        public override string Build()
        {
            var stringBuilder = new StringBuilder("CREATE TRIGGER");

            if (isIfNotExists)
                stringBuilder.Append(" IF NOT EXISTS");

            stringBuilder.Append(" '" + triggerName + "'");
            stringBuilder.Append(" " + triggerOccurence);
            stringBuilder.Append(" " + triggerEvent);
            stringBuilder.Append(" ON '" + targetTableName + "'");

            if (isForEachRow)
                stringBuilder.Append(" FOR REACH ROW");

            if (!string.IsNullOrWhiteSpace(condition))
                stringBuilder.Append(" WHEN " + condition);

            stringBuilder.Append(" BEGIN " + triggerActions + " END");

            return stringBuilder.ToString();
        }
    }
}
