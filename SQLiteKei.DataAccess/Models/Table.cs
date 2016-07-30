namespace SQLiteKei.DataAccess.Models
{
    /// <summary>
    /// A model that represents a database table.
    /// </summary>
    public class Table
    {
        public string DatabaseName { get; set; }

        public string Name { get; set; }

        public string CreateStatement { get; set; }
    }
}
