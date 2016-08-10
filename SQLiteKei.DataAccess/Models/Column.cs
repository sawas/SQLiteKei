namespace SQLiteKei.DataAccess.Models
{
    /// <summary>
    /// A model that represents a database table column.
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Gets or sets the Id of the column.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the column's name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the column.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is defined as NOT NULL.
        /// </summary>
        /// <value>
        /// <c>true</c> if this column is not nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNotNullable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is defined as PRIMARY.
        /// </summary>
        /// <value>
        /// <c>true</c> if this column is primary; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the default value of the column.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public object DefaultValue { get; set; }
    }
}
