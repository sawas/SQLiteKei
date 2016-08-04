using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SQLiteKei.ViewModels.CreatorWindows.TriggerCreatorWindow
{
    public class TriggerCreatorViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private DatabaseSelectItem selectedDatabase;
        public DatabaseSelectItem SelectedDatabase
        {
            get { return selectedDatabase; }
            set { selectedDatabase = value; UpdateAvailableTables(); }
        }

        public List<DatabaseSelectItem> Databases { get; set; }

        private string triggerName;
        public string TriggerName
        {
            get { return triggerName; }
            set { triggerName = value; UpdateModel(); }
        }

        private bool isIfNotExists;
        public bool IsIfNotExists
        {
            get { return isIfNotExists; }
            set { isIfNotExists = value; UpdateSQL(); }
        }

        /// <summary>
        /// The point (in time) when the action takes place, which is either before, after or instead of an event.
        /// </summary>
        private string selectedTriggerEntryPoint;
        public string SelectedTriggerEntryPoint
        {
            get { return selectedTriggerEntryPoint; }
            set { selectedTriggerEntryPoint = value; UpdateModel(); }
        }

        public List<string> TriggerEntryPoints { get; set; }

        private string selectedTriggerEvent;
        public string SelectedTriggerEvent
        {
            get { return selectedTriggerEvent; }
            set { selectedTriggerEvent = value; IsUpdateOfEvent = selectedTriggerEvent.Equals("UPDATE OF"); UpdateModel(); }
        }

        public List<string> TriggerEvents { get; private set; }

        private bool isUpdateOfEvent;
        public bool IsUpdateOfEvent
        {
            get { return isUpdateOfEvent; }
            set { isUpdateOfEvent = value; NotifyPropertyChanged("IsUpdateOfEvent"); }
        }

        private string selectedTarget { get; set; }
        public string SelectedTarget
        {
            get { return selectedTarget; }
            set { selectedTarget = value; UpdateAvailableColumns(); UpdateModel(); }
        }

        public ObservableCollection<string> AvailableTargets { get; set; }

        public ObservableCollection<ColumnItem> Columns { get; set; }

        private bool isForEachRow;
        public bool IsForEachRow
        {
            get { return isForEachRow; }
            set { isForEachRow = value; UpdateSQL(); }
        }

        private string whenExpression;
        public string WhenExpression
        {
            get { return whenExpression; }
            set { whenExpression = value; UpdateModel(); }
        }

        private string triggerActions;
        public string TriggerActions
        {
            get { return triggerActions; }
            set { triggerActions = value; UpdateModel(); }
        }

        private string sql;
        public string Sql
        {
            get { return sql; }
            set { sql = value; NotifyPropertyChanged("SQL"); }
        }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        private bool isValidModel;
        public bool IsValidModel
        {
            get { return isValidModel; }
            set { isValidModel = value;  NotifyPropertyChanged("IsValidModel"); }
        }

        private CreateTriggerQueryBuilder queryBuilder;

        public TriggerCreatorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();

            TriggerEntryPoints = new List<string> { "BEFORE", "AFTER", "INSTEAD OF" };
            TriggerEvents = new List<string> { "DELETE", "INSERT", "UPDATE", "UPDATE OF" };

            AvailableTargets = new ObservableCollection<string>();
            Columns = new ObservableCollection<ColumnItem>();
            Columns.CollectionChanged += CollectionContentChanged;

            createCommand = new DelegateCommand(Create);

            UpdateModel();
        }

        private void CollectionContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (NotifyingModel item in e.OldItems)
                {
                    item.PropertyChanged -= CollectionItemPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (NotifyingModel item in e.NewItems)
                {
                    item.PropertyChanged += CollectionItemPropertyChanged;
                }
            }
        }

        private void CollectionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateSQL();
        }

        private void UpdateAvailableTables()
        {
            if (string.IsNullOrEmpty(SelectedDatabase.DatabasePath)) return;

            using (var dbHandler = new DatabaseHandler(SelectedDatabase.DatabasePath))
            {
                var tables = dbHandler.GetTables();
                var views = dbHandler.GetViews();

                AvailableTargets.Clear();

                foreach (var table in tables)
                {
                    AvailableTargets.Add(table.Name);
                }
                foreach (var view in views)
                {
                    AvailableTargets.Add(view.Name);
                }
            }
        }

        private void UpdateAvailableColumns()
        {
            if (string.IsNullOrEmpty(SelectedDatabase.DatabasePath)) return;

            using (var tableHandler = new TableHandler(SelectedDatabase.DatabasePath))
            {
                Columns.Clear();

                var columns = tableHandler.GetColumns(selectedTarget);

                foreach (var column in columns)
                {
                    Columns.Add(new ColumnItem { ColumnName = column.Name, IsSelected = true });
                }
            }
        }

        private void UpdateModel()
        {
            VerifyModel();
            UpdateSQL();
        }

        private void VerifyModel()
        {
            IsValidModel = selectedDatabase != null
                && !string.IsNullOrEmpty(triggerName)
                && !string.IsNullOrEmpty(selectedTriggerEntryPoint)
                && !string.IsNullOrEmpty(selectedTriggerEvent)
                && !string.IsNullOrEmpty(selectedTarget)
                && !string.IsNullOrEmpty(triggerActions);

            if (!IsValidModel)
                StatusInfo = LocalisationHelper.GetString("TriggerCreator_StatusInfo_InvalidModel");
            else
                StatusInfo = string.Empty;
        }

        private void UpdateSQL()
        {
            queryBuilder = new CreateTriggerQueryBuilder(TriggerName)
                .IfNotExists(IsIfNotExists)
                .On(selectedTarget)
                .ForEachRow(IsForEachRow)
                .When(whenExpression)
                .Do(triggerActions);

            SetEntryPoint();
            SetEvent();

            Sql = queryBuilder.Build();
        }

        private void SetEntryPoint()
        {
            if (string.IsNullOrEmpty(selectedTriggerEntryPoint)) return;

            if (SelectedTriggerEntryPoint.Equals("BEFORE"))
                queryBuilder = queryBuilder.Before();
            else if (SelectedTriggerEntryPoint.Equals("INSTEAD OF"))
                queryBuilder = queryBuilder.InsteadOf();
            else
                queryBuilder = queryBuilder.After();
        }

        private void SetEvent()
        {
            if (string.IsNullOrEmpty(selectedTriggerEvent)) return;

            if (selectedTriggerEvent.Equals("INSERT"))
                queryBuilder = queryBuilder.Insert();
            else if (selectedTriggerEvent.Equals("UPDATE"))
                queryBuilder = queryBuilder.Update();
            else if (selectedTriggerEvent.Equals("UPDATE OF"))
                queryBuilder = queryBuilder.Update(Columns.Where(x => x.IsSelected).Select(x => x.ColumnName).ToList());
            else
                queryBuilder = queryBuilder.Delete();
        }

        private void Create()
        {
            StatusInfo = string.Empty;

            using (var dbHandler = new DatabaseHandler(selectedDatabase.DatabasePath))
            {
                try
                {
                    dbHandler.ExecuteNonQuery(sql);
                    StatusInfo = LocalisationHelper.GetString("TriggerCreator_StatusInfo_Success");
                    MainTreeHandler.AddTrigger(triggerName, selectedDatabase.DatabasePath);
                }
                catch(Exception ex)
                {
                    logger.Error("An error occured when the user tried to create a trigger from the TriggerCreator.", ex);
                    StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                }
            }
        }

        private readonly DelegateCommand createCommand;

        public DelegateCommand CreateCommand { get { return createCommand; } }
    }
}
