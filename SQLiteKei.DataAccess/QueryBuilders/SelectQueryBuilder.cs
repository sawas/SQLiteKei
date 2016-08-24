using System.Collections.Generic;
using System.Linq;
using System.Text;

using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.QueryBuilders.Base;
using SQLiteKei.DataAccess.QueryBuilders.Where;
using SQLiteKei.DataAccess.QueryBuilders.Data;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class SelectQueryBuilder : ConditionalQueryBuilder
    {
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
        }

        public SelectQueryBuilder(string select)
        {
            selects = new Dictionary<string, string>();
            selects.Add(select, string.Empty);
            WhereClauses = new List<string>();
            OrderClauses = new List<OrderData>();
        }

        public SelectQueryBuilder(string select, string alias)
        {
            selects = new Dictionary<string, string>();
            selects.Add(select, alias);
            WhereClauses = new List<string>();
            OrderClauses = new List<OrderData>();
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

        //TODO Unit tests for this one needed
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

        //TODO replace string concatination with StringBuilder
        public override string Build()
        {
            if(string.IsNullOrWhiteSpace(table))
                throw new SelectQueryBuilderException("No table has been defined.");

            string finalSelect = GetFinalSelect();

            string resultString;

            if (isDistinct)
                resultString = string.Format("SELECT DISTINCT {0}\nFROM '{1}'", finalSelect, table);
            else
                resultString = string.Format("SELECT {0}\nFROM '{1}'", finalSelect, table);

            if (WhereClauses.Any())
            {
                var combinedWhereClauses = string.Join("\n", WhereClauses);
                resultString += string.Format("\nWHERE {0}", combinedWhereClauses);
            }

            if (OrderClauses.Any())
            {
                var combinedOrderClauses = string.Join(", ", OrderClauses.Select(x => x.ToString()));
                resultString += string.Format("\nORDER BY {0}", combinedOrderClauses);
            }

            if (limit > 0)
            {
                resultString += string.Format("\nLIMIT {0}", limit);

                if (limitOffset > 0)
                    resultString += string.Format(" OFFSET {0}", limitOffset);
            }

            return resultString;
        }

        private string GetFinalSelect()
        {
            if (!selects.Any())
            {
                return "*";
            }

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

            return string.Join(", ", selectsAndAliases);
        }
    }
}