namespace SQLiteKei.DataAccess.Models
{
    /// <summary>
    /// A model that represents a database index.
    /// </summary>
    public class Index
    {
        /// <summary>
        /// Gets or sets the name of the index.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the table on which the index is set.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the index enforces uniqueness.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance enforces uniqueness; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnique { get; set; }

        /// <summary>
        /// Gets or sets the SQL statement that was used to create the index.
        /// </summary>
        /// <value>
        /// The SQL statement.
        /// </value>
        public string SqlStatement { get; set; }
    }
}
