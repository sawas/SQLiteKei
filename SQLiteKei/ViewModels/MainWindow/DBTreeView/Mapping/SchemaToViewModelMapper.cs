using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;

using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SQLiteKei.ViewModels.MainWindow.DBTreeView.Mapping
{
    /// <summary>
    /// A mapping class that opens a connection to the provided database and builds a hierarchical ViewModel structure.
    /// </summary>
    internal class SchemaToViewModelMapper
    {
        private string databasePath;

        private DatabaseHandler dbHandler;

        private readonly ILog logger = LogHelper.GetLogger();

        /// <summary>
        /// Maps the provided database to a hierarchical ViewModel structure with a DatabaseItem as its root.
        /// </summary>
        /// <param name="databasePath">The database path.</param>
        /// <returns></returns>
        public DatabaseItem MapSchemaToViewModel(string databasePath)
        {
            logger.Info("Trying to load database file at " + databasePath);
            this.databasePath = databasePath;
            dbHandler = new DatabaseHandler(databasePath);

            TableFolderItem tableFolder = MapTables();
            ViewFolderItem viewFolder = MapViews();
            IndexFolderItem indexFolder = MapIndexes();
            TriggerFolderItem triggerFolder = MapTriggers();

            var databaseItem = new DatabaseItem()
            {
                DisplayName = Path.GetFileNameWithoutExtension(databasePath),
                DatabasePath = databasePath
            };

            databaseItem.Items.Add(tableFolder);
            databaseItem.Items.Add(viewFolder);
            databaseItem.Items.Add(indexFolder);
            databaseItem.Items.Add(triggerFolder);

            logger.Info("Loaded database " + databaseItem.DisplayName + ".");
            return databaseItem;
        }

        private TableFolderItem MapTables()
        {
            var tables = dbHandler.GetTables();
            var tableFolder = new TableFolderItem { DisplayName = LocalisationHelper.GetString("TreeItem_Tables") };

            foreach (var table in tables)
            {
                tableFolder.Items.Add(new TableItem
                {
                    DisplayName = table.Name,
                    DatabasePath = databasePath
                });
            }
            tableFolder.Items = new ObservableCollection<TreeItem>(tableFolder.Items.OrderBy(x => x.DisplayName));

            return tableFolder;
        }

        private ViewFolderItem MapViews()
        {
            var views = dbHandler.GetViews();
            var viewFolder = new ViewFolderItem { DisplayName = LocalisationHelper.GetString("TreeItem_Views") };

            foreach (var view in views)
            {
                viewFolder.Items.Add(new ViewItem
                {
                    DisplayName = view.Name,
                    DatabasePath = databasePath
                });
            }
            viewFolder.Items = new ObservableCollection<TreeItem>(viewFolder.Items.OrderBy(x => x.DisplayName));

            return viewFolder;
        }

        private IndexFolderItem MapIndexes()
        {
            var indexes = dbHandler.GetIndexes();
            var indexFolder = new IndexFolderItem { DisplayName = LocalisationHelper.GetString("TreeItem_Indexes") };

            foreach (var index in indexes)
            {
                indexFolder.Items.Add(new IndexItem
                {
                    DisplayName = index.Name,
                    DatabasePath = databasePath
                });
            }
            indexFolder.Items = new ObservableCollection<TreeItem>(indexFolder.Items.OrderBy(x => x.DisplayName));

            return indexFolder;
        }

        private TriggerFolderItem MapTriggers()
        {
            var triggers = dbHandler.GetTriggers();
            var triggerFolder = new TriggerFolderItem { DisplayName = LocalisationHelper.GetString("TreeItem_Triggers") };

            foreach (var trigger in triggers)
            {
                triggerFolder.Items.Add(new TriggerItem
                {
                    DisplayName = trigger.Name,
                    DatabasePath = databasePath
                });
            }
            triggerFolder.Items = new ObservableCollection<TreeItem>(triggerFolder.Items.OrderBy(x => x.DisplayName));

            return triggerFolder;
        }
    }
}
