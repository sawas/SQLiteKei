using System.Collections.Generic;
using System.Linq;
using System.Text;

using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.QueryBuilders.Base;
using SQLiteKei.DataAccess.QueryBuilders.Where;
using SQLiteKei.DataAccess.QueryBuilders.Data;
using System;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class SelectQueryBuilder : ConditionalQueryBuilder
    {
        private StringBuilder resultStringBuilder;

        private string table;

        private bool isDistinct;

        private Dictionary<string, string> selects;

        private List<OrderData> OrderClauses { get; set; }

        private long limit;

        private long limitOffset;

        public SelectQueryBuilder()
        {
            selects = new Dictionary<string, string>();
            WhereClauses = new List<string>();
            OrderClauses = new List<OrderData>();
            resultStringBuilder = new StringBuilder();
        }

        public SelectQueryBuilder(string select) : this()
        {
            selects.Add(select, string.Empty);
        }

        public SelectQueryBuilder(string select, string alias) : this()
        {
            selects.Add(select, alias);
        }

        public SelectQueryBuilder Distinct(bool value = true)
        {
            isDistinct = value;
            return this;
        }

        public SelectQueryBuilder AddSelect(string select)
        {
            selects.Add(select, string.Empty);
            return this;
        }

        public SelectQueryBuilder AddSelect(string select, string alias)
        {
            selects.Add(select, alias);
            return this;
        }

        public SelectQueryBuilder From(string tableName)
        {
            table = tableName;
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

        public override SelectQueryBuilder OrderBy(string columnName, bool descending = false)
        {
            OrderClauses.Add(new OrderData
            {
                ColumnName = columnName,
                IsDescending = descending
            });

            return this;
        }

        public SelectQueryBuilder Limit(long limit, long offset = 0)
        {
            this.limit = limit;
            this.limitOffset = offset;

            return this;
        }

        public override string Build()
        {
            if(string.IsNullOrWhiteSpace(table))
                throw new SelectQueryBuilderException("No table has been defined.");

            AppendSelects();
            AppendWhereClauses();
            AppendOrderClauses();
            AppendLimit();

            return resultStringBuilder.ToString();
        }

        private void AppendSelects()
        {
            string finalSelect = GetFinalSelect();

            if (isDistinct)
                resultStringBuilder.Append(string.Format("SELECT DISTINCT {0}\nFROM '{1}'", finalSelect, table));
            else
                resultStringBuilder.Append(string.Format("SELECT {0}\nFROM '{1}'", finalSelect, table));
        }

        private string GetFinalSelect()
        {
            if (!selects.Any())
            {
                return "*";
            }

            var selectsAndAliases = CombineSelectsAndAliases();        
            return string.Join(", ", selectsAndAliases);
        }

        private IEnumerable<string> CombineSelectsAndAliases()
        {
            var selectsAndAliases = new List<string>();

            foreach (var select in selects)
            {
                var stringBuilder = new StringBuilder(select.Key);

                if (!string.IsNullOrWhiteSpace(select.Value))
                {
                    stringBuilder.Append(" AS ");
                    stringBuilder.Append(select.Value);
                }

                selectsAndAliases.Add(stringBuilder.ToString());
            }

            return selectsAndAliases;
        }

        private void AppendOrderClauses()
        {
            if (OrderClauses.Any())
            {
                var combinedOrderClauses = string.Join(", ", OrderClauses.Select(x => x.ToString()));
                resultStringBuilder.Append(string.Format("\nORDER BY {0}", combinedOrderClauses));
            }
        }

        private void AppendWhereClauses()
        {
            if (WhereClauses.Any())
            {
                var combinedWhereClauses = string.Join("\n", WhereClauses);
                resultStringBuilder.Append(string.Format("\nWHERE {0}", combinedWhereClauses));
            }
        }

        private void AppendLimit()
        {
            if (limit > 0)
            {
                resultStringBuilder.Append(string.Format("\nLIMIT {0}", limit));

                if (limitOffset > 0)
                    resultStringBuilder.Append(string.Format(" OFFSET {0}", limitOffset));
            }
        }
    }
}