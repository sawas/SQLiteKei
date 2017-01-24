using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Util;
using SQLiteKei.Util.Interfaces;

using System;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Databases
{
    /// <summary>
    /// A ViewModel that is used in the main tab view to display general table data when a database item is selected.
    /// </summary>
    public class TableOverviewDataItem
    {
        private ILog logger = LogHelper.GetLogger();

        private IDialogService dialogService = new DialogService();

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
                        dialogService.ShowMessage("MessageBox_NameChangeFailed");
                    }
                }
            }
        }

        public long RowCount { get; set; }

        public int ColumnCount { get; set; }
    }
}
