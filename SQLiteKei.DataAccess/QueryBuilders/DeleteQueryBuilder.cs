using SQLiteKei.DataAccess.QueryBuilders.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteKei.DataAccess.QueryBuilders.Where;

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
        }

        public override WhereClause And(string columnName)
        {
            throw new NotImplementedException();
        }

        public override WhereClause Or(string columnName)
        {
            throw new NotImplementedException();
        }

        public override SelectQueryBuilder OrderBy(string columnName, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public override WhereClause Where(string columnName)
        {
            throw new NotImplementedException();
        }

        internal override void AddWhereClause(string where)
        {
            throw new NotImplementedException();
        }

        public override string Build()
        {
            throw new NotImplementedException();
        }
    }
}
