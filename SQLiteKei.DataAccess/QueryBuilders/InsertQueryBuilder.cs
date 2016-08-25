using SQLiteKei.DataAccess.QueryBuilders.Base;

using System.Collections.Generic;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class InsertQueryBuilder : QueryBuilderBase
    {
        private readonly string tableName;

        private List<string> values;

        public InsertQueryBuilder(string tableName)
        {
            this.tableName = tableName;
            values = new List<string>();
        }

        public InsertQueryBuilder Values(IEnumerable<string> values)
        {
            this.values.Clear();

            foreach(var value in values)
            {
                this.values.Add("\"" + value + "\"");
            }

            return this;
        }

        public override string Build()
        {
            var valueList = string.Join(",", values);

            return string.Format("INSERT INTO '{0}' VALUES ({1});", tableName, valueList);
        }
    }
}
