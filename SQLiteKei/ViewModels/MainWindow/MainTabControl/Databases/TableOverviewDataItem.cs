using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Util;

using System;
using System.Windows;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Databases
{
    /// <summary>
    /// A ViewModel that is used in the main tab view to display general table data when a database item is selected.
    /// </summary>
    public class TableOverviewDataItem
    {
        private ILog logger = LogHelper.GetLogger();

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = value;
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
                        {
                            tableHandler.RenameTable(name, value);
                            MainTreeHandler.UpdateTableName(name, value, Properties.Settings.Default.CurrentDatabase);
                            name = value;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Failed to rename table '" + name + "' from database overview.", ex);
                        var errorMessage = LocalisationHelper.GetString("MessageBox_NameChangeWarning", ex.Message);

                        MessageBox.Show(errorMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        public long RowCount { get; set; }

        public int ColumnCount { get; set; }
    }
}
