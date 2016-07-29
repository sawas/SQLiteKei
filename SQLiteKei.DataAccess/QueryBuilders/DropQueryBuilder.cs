using System.Text;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class DropTableQueryBuilder
    {
        private string table;

        private bool isCascade;

        private bool isIfExists;

        public DropTableQueryBuilder(string tableName)
        {
            table = tableName;
        }

        public DropTableQueryBuilder IfExists()
        {
            isIfExists = true;

            return this;
        }

        public DropTableQueryBuilder Cascade()
        {
            isCascade = true;

            return this;
        }

        public string Build()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("DROP TABLE");

            if (isIfExists)
                stringBuilder.Append(" IF EXISTS");

            stringBuilder.Append(" '" + table + "'");

            if (isCascade)
                stringBuilder.Append(" CASCADE");

            return stringBuilder.ToString();
        }
    }
}
