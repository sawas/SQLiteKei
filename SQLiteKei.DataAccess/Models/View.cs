namespace SQLiteKei.DataAccess.Models
{
    /// <summary>
    /// A model that represents a database view.
    /// </summary>
    public class View
    {
        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        /// <value>
        /// The view name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the views SQL statement.
        /// </summary>
        /// <value>
        /// The SQL statement.
        /// </value>
        public string SqlStatement { get; set; }
    }
}
