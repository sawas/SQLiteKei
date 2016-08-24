﻿using SQLiteKei.DataAccess.QueryBuilders.Base;
using SQLiteKei.DataAccess.QueryBuilders.Where;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class UpdateQueryBuilder : ConditionalQueryBuilder
    {
        private readonly string tableName;

        private List<string> sets;

        private List<string> whereClauses;

        public UpdateQueryBuilder(string tableName)
        {
            this.tableName = tableName;
            sets = new List<string>();
            whereClauses = new List<string>();

        }

        public UpdateQueryBuilder Set(string columnName, string value)
        {
            sets.Add(string.Format("{0}='{1}'", columnName, value));
            return this;
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

        [Obsolete("Don't use")]
        public override SelectQueryBuilder OrderBy(string columnName, bool descending = false)
        {
            return null;
        }

        internal override void AddWhereClause(string where)
        {
            whereClauses.Add(where);
        }

        public override string Build()
        {
            var combinedSets = string.Join(", ", sets);

            if (!whereClauses.Any())
                return string.Format("UPDATE {0}\nSET {1}", tableName, combinedSets);

            var combinedWhereClauses = string.Join("\n", whereClauses);
            return string.Format("UPDATE {0}\nSET {1}\nWHERE {2}", tableName, combinedSets, combinedWhereClauses);
        }
    }
}
