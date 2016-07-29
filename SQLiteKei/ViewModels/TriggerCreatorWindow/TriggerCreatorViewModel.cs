using log4net;

using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;
using SQLiteKei.DataAccess.Helpers;
using SQLiteKei.DataAccess.Database;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using SQLiteKei.DataAccess.QueryBuilders;
using System;
using System.Linq;
using SQLiteKei.Commands;

namespace SQLiteKei.ViewModels.TriggerCreatorWindow
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
            set { triggerName = value; UpdateSQL(); }
        }

        /// <summary>
        /// The point (in time) when the action takes place, which is either before, after or instead of an event.
        /// </summary>
        /// <value>
        /// The selected trigger entry point.
        /// </value>
        public string SelectedTriggerEntryPoint { get; set; }

        public List<string> TriggerEntryPoints { get; set; }

        private string selectedTriggerEvent;
        public string SelectedTriggerEvent
        {
            get { return selectedTriggerEvent; }
            set { selectedTriggerEvent = value; IsUpdateOfEvent = selectedTriggerEvent.Equals("UPDATE OF"); }
        }

        public List<string> TriggerEvents { get; private set; }

        private bool isUpdateOfEvent;
        public bool IsUpdateOfEvent
        {
            get { return isUpdateOfEvent; }
            set { isUpdateOfEvent = value; NotifyPropertyChanged("IsUpdateOfEvent"); }
        }

        private string selectedTable { get; set; }
        public string SelectedTable
        {
            get { return selectedTable; }
            set { selectedTable = value; UpdateAvailableColumns(); }
        }

        public ObservableCollection<string> AvailableTables { get; set; }

        public ObservableCollection<ColumnItem> Columns { get; set; }

        private CreateTriggerQueryBuilder queryBuilder;

        public TriggerCreatorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();

            TriggerEntryPoints = new List<string> { "BEFORE", "AFTER", "INSTEAD OF" };
            TriggerEvents = new List<string> { "DELETE", "INSERT", "UPDATE", "UPDATE OF" };

            AvailableTables = new ObservableCollection<string>();
            Columns = new ObservableCollection<ColumnItem>();
            Columns.CollectionChanged += CollectionContentChanged;

            createCommand = new DelegateCommand(UpdateSQL);
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
                {
                    AvailableTables.Clear();

                    foreach (var table in tables)
                    {
                        AvailableTables.Add(table.Name);
                    }
                }
            }
        }

        private void UpdateAvailableColumns()
        {
            if (string.IsNullOrEmpty(SelectedDatabase.DatabasePath)) return;

            using (var tableHandler = new TableHandler(SelectedDatabase.DatabasePath))
            {
                Columns.Clear();

                var columns = tableHandler.GetColumns(selectedTable);

                foreach (var column in columns)
                {
                    Columns.Add(new ColumnItem { ColumnName = column.Name, IsSelected = true });
                }
            }
        }

        private void UpdateSQL()
        {
            queryBuilder = new CreateTriggerQueryBuilder(TriggerName);
            SetEntryPoint();
            SetEvent();

        }

        private void SetEntryPoint()
        {
            if (SelectedTriggerEntryPoint.Equals("BEFORE"))
                queryBuilder = queryBuilder.Before();
            else if (SelectedTriggerEntryPoint.Equals("INSTEAD OF"))
                queryBuilder = queryBuilder.InsteadOf();
            else
                queryBuilder = queryBuilder.After();
        }

        private void SetEvent()
        {
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

        }

        private readonly DelegateCommand createCommand;

        public DelegateCommand CreateCommand { get { return createCommand; } }
    }
}
