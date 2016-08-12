using System.Text;

namespace SQLiteKei.DataAccess.QueryBuilders.Data
{
    /// <summary>
    /// Data that is used by the CreateQueryBuilder to create the column creation statements.
    /// </summary>
    public class ColumnData
    {
        public string ColumnName { get; set; }

        public string DataType { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsNotNull { get; set; }

        public object DefaultValue { get; set; }

        public override string ToString()
        {
            var column = new StringBuilder(ColumnName + " " + DataType);

            if (IsPrimary)
                column.Append(" PRIMARY KEY");

            if (IsNotNull || IsPrimary)
                column.Append(" NOT NULL");

            if (DefaultValue != null && !string.IsNullOrEmpty(DefaultValue.ToString()))
                column.Append(" DEFAULT '" + DefaultValue.ToString() + "'");

            return column.ToString();
        }
    }
}
