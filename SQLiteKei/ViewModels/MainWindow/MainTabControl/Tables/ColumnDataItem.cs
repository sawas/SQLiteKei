using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Util;
using SQLiteKei.Util.Interfaces;

using System;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Tables
{
    public class ColumnDataItem
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
                    var userAgrees = dialogService.AskForUserAgreement("MessageBox_ColumnRenameWarning", "MessageBoxTitle_RenameColumn", name);
                    if (!userAgrees) return;

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
                        dialogService.ShowMessage("MessageBox_NameChangeFailed");
                    }
                }
            }
        }

        public string TableName { get; set; }

        public string DataType { get; set; }

        public bool IsNotNullable { get; set; }

        public object DefaultValue { get; set; }

        public bool IsPrimary { get; set; }
    }
}
