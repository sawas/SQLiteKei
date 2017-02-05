using SQLiteKei.DataAccess.QueryBuilders.Base;
using System;

using SQLiteKei.DataAccess.QueryBuilders.Where;
using System.Collections.Generic;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteQueryBuilder : ConditionalQueryBuilder
    {
        private readonly string tableName;

        public DeleteQueryBuilder(string tableName)
        {
            this.tableName = tableName;
            WhereClauses = new List<string>();
        }

        public override WhereClause Where(string columnName)
        {
            return new WhereClause(this, columnName);
        }

        public override WhereClause Or(string columnName)
        {
            return new OrWhereClause(this, columnName);
        }

        public override WhereClause And(string columnName)
        {
            return new AndWhereClause(this, columnName);
        }

        public override string Build()
        {
            var combinedWhereClauses = string.Join("\nAND ", WhereClauses);

            return string.Format("DELETE FROM '{0}'\nWHERE {1}", tableName, combinedWhereClauses);
        }
    }
}
