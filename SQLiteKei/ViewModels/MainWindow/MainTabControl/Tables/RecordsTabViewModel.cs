using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.DataTypes.Collections;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.SelectQueryWindow;

using System;
using System.Collections.Generic;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Tables
{
    public class RecordsTabViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private string tableName;

        private PagableCollectionView dataGridCollection;
        public PagableCollectionView DataGridCollection
        {
            get { return dataGridCollection; }
            set { dataGridCollection = value; NotifyPropertyChanged("DataGridCollection"); }
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

            DataGridCollection = new PagableCollectionView(new List<object>());

            selectCommand = new DelegateCommand(Select);
            selectAllCommand = new DelegateCommand(SelectAll);
            gotoNextCommand = new DelegateCommand(GotoNextPage);
            gotoPreviousCommand = new DelegateCommand(GotoPreviousPage);
            gotoLastCommand = new DelegateCommand(GotoLastPage);
            gotoFirstCommand = new DelegateCommand(GotoFirstPage);
        }

        public int ItemsPerPage
        {
            get { return DataGridCollection.ItemsPerPage; }
            set { DataGridCollection.ItemsPerPage = value; NotifyPropertyChanged("TotalPages"); NotifyPropertyChanged("CurrentPage"); }
        }

        public int CurrentPage
        {
            get { return DataGridCollection.PageCount == 0 ? 0 : DataGridCollection.CurrentPage; }
        }

        public int TotalPages
        {
            get { return DataGridCollection.PageCount; }
        }

        private void GotoNextPage()
        {
            DataGridCollection.MoveToNextPage();
            NotifyPropertyChanged("CurrentPage");
        }

        private DelegateCommand gotoNextCommand;

        public DelegateCommand GotoNextCommand { get { return gotoNextCommand; } }

        private void GotoPreviousPage()
        {
            DataGridCollection.MoveToPreviousPage();
            NotifyPropertyChanged("CurrentPage");
        }

        private DelegateCommand gotoPreviousCommand;

        public DelegateCommand GotoPreviousCommand { get { return gotoPreviousCommand; } }

        private void GotoLastPage()
        {
            DataGridCollection.MoveToLastPage();
            NotifyPropertyChanged("CurrentPage");
        }

        private DelegateCommand gotoLastCommand;

        public DelegateCommand GotoLastCommand { get { return gotoLastCommand; } }

        private void GotoFirstPage()
        {
            DataGridCollection.MoveToFirstPage();
            NotifyPropertyChanged("CurrentPage");
        }

        private DelegateCommand gotoFirstCommand;

        public DelegateCommand GotoFirstCommand { get { return gotoFirstCommand; } }

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

                    DataGridCollection = new PagableCollectionView(resultTable.DefaultView, ItemsPerPage);
                    StatusInfo = string.Format("Rows returned: {0}", resultTable.Rows.Count);
                    NotifyPropertyChanged("CurrentPage");
                    NotifyPropertyChanged("TotalPages");
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
