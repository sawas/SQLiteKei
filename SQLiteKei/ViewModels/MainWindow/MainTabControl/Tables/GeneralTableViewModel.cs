using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Tables
{
    public class GeneralTableViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private string tableName;
        public string TableName
        {
            get { return tableName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
                        {
                            tableHandler.RenameTable(tableName, value);
                            MainTreeHandler.UpdateTableName(tableName, value, Properties.Settings.Default.CurrentDatabase);
                            tableName = value;
                            TableCreateStatement = tableHandler.GetTable(tableName).CreateStatement;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Failed to rename table '" + tableName + "' from table overview.", ex);
                        var message = LocalisationHelper.GetString("MessageBox_NameChangeWarning", ex.Message);

                        MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        private long rowCount;
        public long RowCount
        {
            get { return rowCount; }
            set { rowCount = value; NotifyPropertyChanged("RowCount"); }
        }

        private int columnCount;
        public int ColumnCount
        {
            get { return columnCount; }
            set { columnCount = value; NotifyPropertyChanged("ColumnCount"); }
        }

        public bool ColumnsFound { get; set; }

        private string tableCreateStatement;
        public string TableCreateStatement
        {
            get { return tableCreateStatement; }
            set { tableCreateStatement = value; NotifyPropertyChanged("TableCreateStatement"); }
        }

        private ColumnDataItem selectedColumn;
        public ColumnDataItem SelectedColumn
        {
            get { return selectedColumn; }
            set { selectedColumn = value; }
        }

        public ObservableCollection<ColumnDataItem> Columns { get; set; }

        public GeneralTableViewModel(string tableName)
        {
            Columns = new ObservableCollection<ColumnDataItem>();
            this.tableName = tableName;

            Initialize();

            emptyCommand = new DelegateCommand(EmptyTable);
            reindexCommand = new DelegateCommand(ReindexTable);
            deleteColumnCommand = new DelegateCommand(DeleteColumn);
        }

        private void Initialize()
        {
            using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
            {
                var table = tableHandler.GetTable(tableName);
                TableCreateStatement = table.CreateStatement;
                RowCount = tableHandler.GetRowCount(TableName);
                var columns = tableHandler.GetColumns(TableName);
                ColumnCount = columns.Count;

                foreach (var column in columns)
                {
                    Columns.Add(MapToColumnData(column));
                }
            }

            if (Columns.Any())
                ColumnsFound = true;
        }

        private ColumnDataItem MapToColumnData(Column column)
        {
            return new ColumnDataItem
            {
                Name = column.Name,
                DataType = column.DataType,
                IsNotNullable = column.IsNotNullable,
                IsPrimary = column.IsPrimary,
                DefaultValue = column.DefaultValue
            };
        }

        private void EmptyTable()
        {
            var message = LocalisationHelper.GetString("MessageBox_EmptyTable", tableName);
            var messageTitle = LocalisationHelper.GetString("MessageBoxTitle_EmptyTable");
            var result = MessageBox.Show(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
            {
                tableHandler.EmptyTable(TableName);
                RowCount = 0;
            }
        }

        private DelegateCommand emptyCommand;
        
        public DelegateCommand EmptyCommand { get { return emptyCommand; } }

        private void ReindexTable()
        {
            var message = LocalisationHelper.GetString("MessageBox_ReindexTable", tableName);
            var messageTitle = LocalisationHelper.GetString("MessageBoxTitle_ReindexTable");
            var result = MessageBox.Show(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
            {
                tableHandler.ReindexTable(TableName);
            }
        }

        private DelegateCommand reindexCommand;

        public DelegateCommand ReindexCommand { get { return reindexCommand; } }

        private void DeleteColumn()
        {
            if (selectedColumn != null)
            {
                var message = LocalisationHelper.GetString("MessageBox_ColumnDeleteWarning", selectedColumn.Name);
                var messageTitle = LocalisationHelper.GetString("MessageBoxTitle_DeleteColumn");
                var result = MessageBox.Show(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes) return;

                try
                {
                    using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
                    {
                        tableHandler.DeleteColumn(TableName, selectedColumn.Name);
                        Columns.Remove(selectedColumn);
                        ColumnCount--;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Failed to delete column '" + selectedColumn.Name + "' on table '" + tableName + "'.", ex);
                    var errorMessage = LocalisationHelper.GetString("MessageBox_ColumnDeletionFailed", ex.Message);

                    MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                };
                
            }
        }

        private DelegateCommand deleteColumnCommand;

        public DelegateCommand DeleteColumnCommand { get { return deleteColumnCommand; } }
    }
}
