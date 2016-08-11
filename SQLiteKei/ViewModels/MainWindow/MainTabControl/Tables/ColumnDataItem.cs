using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Util;

using System;
using System.Windows;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Tables
{
    public class ColumnDataItem
    {
        private ILog logger = LogHelper.GetLogger();

        private string name;
        public string Name
        {
            get { return name; }
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
                        {
                            tableHandler.RenameColumn(name, value, TableName);
                            name = value;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Failed to rename column '" + name + "' from table overview.", ex);
                        var message = LocalisationHelper.GetString("MessageBox_NameChangeWarning", ex.Message);

                        MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the table on which the column is defined.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; set; }

        public string DataType { get; set; }

        public bool IsNotNullable { get; set; }

        public object DefaultValue { get; set; }

        public bool IsPrimary { get; set; }
    }
}
