using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.Extensions;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.SelectQueryWindow;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Data;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Tables
{
    public class RecordsTabViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private string tableName;

        private ICollectionView dataGridCollection;
        public ICollectionView DataGridCollection
        {
            get { return dataGridCollection; }
            set
            {
                dataGridCollection = value;
                if (dataGridCollection.CanFilter)
                    dataGridCollection.Filter = Filter;
                NotifyPropertyChanged("DataGridCollection"); 
            }
        }

        private string searchString;
        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                NotifyPropertyChanged("SearchString");
                FilterCollection();
            }
        }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        public RecordsTabViewModel(string tableName)
        {
            this.tableName = tableName;

            DataGridCollection = new ListCollectionView(new List<object>());

            selectCommand = new DelegateCommand(Select);
            selectAllCommand = new DelegateCommand(SelectAll);
        }

        private void FilterCollection()
        {
            if (dataGridCollection != null)
            {
                dataGridCollection.Refresh();
            }
        }

        private bool Filter(object obj)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return true;
            }

            var rowView = obj as DataRowView;
            var row = rowView.Row;

            return row.ItemArray.Select(entry => entry.ToString())
                .Any(value => value.Contains(searchString, StringComparison.OrdinalIgnoreCase));
        }

        private void Select()
        {
            var createSelectWindow = new SQLiteKei.Views.Windows.SelectQueryWindow(tableName);

            if (createSelectWindow.ShowDialog().Value)
            {
                StatusInfo = string.Empty;
                var windowViewModel = createSelectWindow.DataContext as SelectQueryViewModel;

                logger.Info("Executing Select command on table records tab:" 
                    + Environment.NewLine 
                    + windowViewModel.SelectQuery.Replace("\n", Environment.NewLine));
                Execute(windowViewModel.SelectQuery);
            }
        }

        private DelegateCommand selectCommand;

        public DelegateCommand SelectCommand { get { return selectCommand; } }

        private void SelectAll()
        {
            var query = QueryBuilder.Select().From(tableName).Build();

            logger.Info("Executing SelectAll command on table records tab.");
            Execute(query);
        }

        private DelegateCommand selectAllCommand;

        public DelegateCommand SelectAllCommand { get { return selectAllCommand; } }

        private void Execute(string selectQuery)
        {
            try
            {
                var dbPath = Properties.Settings.Default.CurrentDatabase;
                using (var dbHandler = new DatabaseHandler(dbPath))
                {
                    var resultTable = dbHandler.ExecuteReader(selectQuery);

                    DataGridCollection = new ListCollectionView(resultTable.DefaultView);
                    StatusInfo = string.Format("Rows returned: {0}", resultTable.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Select query failed.", ex);
                StatusInfo = ex.Message.Replace("SQL logic error or missing database ", "SQL Error - ");
            }
        }
    }
}
