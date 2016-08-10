namespace SQLiteKei.DataAccess.Models
{
    /// <summary>
    /// A model that represents a database table.
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Gets or sets the name of the on which the table was created.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the name of the table..
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SQL statement that was used to create the table.
        /// </summary>
        /// <value>
        /// The SQL statement.
        /// </value>
        public string CreateStatement { get; set; }
    }
}
