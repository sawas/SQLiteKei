﻿using SQLiteKei.DataAccess.QueryBuilders.Where;
using System.Collections.Generic;

namespace SQLiteKei.DataAccess.QueryBuilders.Base
{
    public abstract class ConditionalQueryBuilder : QueryBuilderBase
    {
        public List<string> WhereClauses { get; set; }

        internal void AddWhereClause(string where)
        {
            WhereClauses.Add(where);
        }

        public abstract WhereClause Where(string columnName);

        public abstract WhereClause Or(string columnName);

        public abstract WhereClause And(string columnName);

        //TODO because of this method, the whole thing needs to be reevaluated/refactored, maybe select-specific where-clauses
        public abstract SelectQueryBuilder OrderBy(string columnName, bool descending = false);
    }
}