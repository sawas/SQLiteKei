using SQLiteKei.DataAccess.QueryBuilders.Base;
using SQLiteKei.DataAccess.QueryBuilders.Enums;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class CreateIndexBuilder : QueryBuilderBase
    {
        private string indexName;

        private bool isUnique;

        private bool isIfNotExists;

        private string targetTable;

        private List<string> indexedColumns;

        private string whereStatement;

        public CreateIndexBuilder(string indexName)
        {
            this.indexName = indexName;

            indexedColumns = new List<string>();
        }

        public CreateIndexBuilder Unique(bool value = true)
        {
            isUnique = value;
            return this;
        }

        public CreateIndexBuilder IfNotExists(bool value = true)
        {
            isIfNotExists = value;
            return this;
        }

        public CreateIndexBuilder On(string tableName)
        {
            targetTable = tableName;
            return this;
        }

        public CreateIndexBuilder IndexColumn(string columnName, OrderType orderType)
        {
            switch(orderType)
            {
                case OrderType.Ascending:
                    indexedColumns.Add(string.Format("'{0}' ASC", columnName));
                    break;
                case OrderType.Descending:
                    indexedColumns.Add(string.Format("'{0}' DESC", columnName));
                    break;
            }

            return this;
        }

        public CreateIndexBuilder Where(string statement)
        {
            whereStatement = statement;
            return this;
        }

        public override string Build()
        {
            var stringBuilder = new StringBuilder("CREATE");

            if (isUnique)
                stringBuilder.Append(" UNIQUE");

            stringBuilder.Append(" INDEX");

            if (isIfNotExists)
                stringBuilder.Append(" IF NOT EXISTS");

            stringBuilder.Append(" '" + indexName + "'");
            stringBuilder.Append("\nON '" + targetTable + "'");

            if(indexedColumns.Any())
            {
                var columnsToAdd = string.Join(", ", indexedColumns);
                stringBuilder.Append(string.Format(" ({0})", columnsToAdd));
            }

            if(!string.IsNullOrWhiteSpace(whereStatement))
                stringBuilder.Append("\nWHERE " + whereStatement);

            return stringBuilder.ToString();
        }
    }
}
