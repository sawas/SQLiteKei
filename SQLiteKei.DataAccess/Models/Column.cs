namespace SQLiteKei.DataAccess.Models
{
    /// <summary>
    /// A model that represents a database table column.
    /// </summary>
    public class Column
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DataType { get; set; }

        public bool IsNotNullable { get; set; }

        public object DefaultValue { get; set; }

        public bool IsPrimary { get; set; }
    }
}
