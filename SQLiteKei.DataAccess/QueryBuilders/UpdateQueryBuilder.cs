﻿using SQLiteKei.DataAccess.QueryBuilders.Base;
using SQLiteKei.DataAccess.QueryBuilders.Where;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// A builder class that allows the creation of UPDATE queries.
    /// </summary>
    public class UpdateQueryBuilder : ConditionalQueryBuilder
    {
        private readonly string tableName;

        private List<string> sets;

        public UpdateQueryBuilder(string tableName)
        {
            this.tableName = tableName;
            sets = new List<string>();
            WhereClauses = new List<string>();
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

        public override string Build()
        {
            var combinedSets = string.Join(", ", sets);

            if (!WhereClauses.Any())
                return $"UPDATE '{tableName}'\nSET {combinedSets}";

            var combinedWhereClauses = string.Join("\n", WhereClauses);
            return $"UPDATE '{tableName}'\nSET {combinedSets}\nWHERE {combinedWhereClauses}";
        }
    }
}
