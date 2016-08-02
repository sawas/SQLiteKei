namespace SQLiteKei.DataAccess.Models
{
    /// <summary>
    /// A model that represents a database trigger.
    /// </summary>
    public class Trigger
    {
        /// <summary>
        /// Gets or sets the name of the trigger.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the target view or table of the trigger.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the SQL statement of the trigger.
        /// </summary>
        /// <value>
        /// The SQL statement.
        /// </value>
        public string SqlStatement { get; set; }
    }
}
