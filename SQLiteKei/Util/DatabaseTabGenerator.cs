using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using SQLiteKei.ViewModels.MainWindow.MainTabControl.Databases;
using SQLiteKei.ViewModels.MainWindow.MainTabControl.Indexes;
using SQLiteKei.ViewModels.MainWindow.MainTabControl.Tables;
using SQLiteKei.ViewModels.MainWindow.MainTabControl.Trigger;
using SQLiteKei.ViewModels.MainWindow.MainTabControl.Views;
using SQLiteKei.Views.UserControls;

using System.Collections.Generic;
using System.Windows.Controls;

namespace SQLiteKei.Util
{
    /// <summary>
    /// A class that generates tabs for the currently selected tree item in the main tree.
    /// </summary>
    static class DatabaseTabGenerator
    {
        /// <summary>
        /// Generates the tabs for the specified tree item depending on its type.
        /// </summary>
        /// <param name="treeItem">The tree item.</param>
        /// <returns></returns>
        public static List<TabItem> GenerateTabsFor(TreeItem treeItem)
        {
            if (treeItem == null)
                return GenerateDefaultTabs();

            if (treeItem.GetType() == typeof(DatabaseItem))
                return GenerateDatabaseTabs((DatabaseItem)treeItem);
            if (treeItem.GetType() == typeof(TableItem))
                return GenerateTableTabs((TableItem)treeItem);
            if (treeItem.GetType() == typeof(ViewItem))
                return GenerateViewTabs((ViewItem)treeItem);
            if (treeItem.GetType() == typeof(IndexItem))
                return GenerateIndexTabs((IndexItem)treeItem);
            if (treeItem.GetType() == typeof(TriggerItem))
                return GenerateTriggerTabs((TriggerItem)treeItem);
            return GenerateDefaultTabs();
        }

        /// <summary>
        /// Generates the default tabs.
        /// </summary>
        /// <returns></returns>
        public static List<TabItem> GenerateDefaultTabs()
        {
            return new List<TabItem>();
        }

        private static List<TabItem> GenerateDatabaseTabs(DatabaseItem databaseItem)
        {
            var generalTab = new TabItem
            {
                Header = databaseItem.DisplayName,
                Content = new DatabaseGeneralTabContent
                {
                    DatabaseInfo = new GeneralDatabaseViewModel(databaseItem.DatabasePath)
                }
            };

            return new List<TabItem> { generalTab };
        }

        private static List<TabItem> GenerateTableTabs(TableItem tableItem)
        {
            var generalTab = new TabItem
            {
                Header = LocalisationHelper.GetString("TabHeader_GeneralTable", tableItem.DisplayName),
                Content = new TableGeneralTabContent(new GeneralTableViewModel(tableItem.DisplayName))
            };

            var recordsTab = new TabItem
            {
                Header = LocalisationHelper.GetString("TabHeader_TableRecords"),
                Content = new TableRecordsTabContent
                {
                    TableInfo = new TableRecordsDataItem(tableItem.DisplayName)
                }
            };

            return new List<TabItem> { generalTab, recordsTab };
        }

        private static List<TabItem> GenerateViewTabs(ViewItem viewItem)
        {
            var generalTab = new TabItem
            {
                Header = LocalisationHelper.GetString("TabHeader_GeneralView", viewItem.DisplayName),
                Content = new ViewGeneralTabContent(new GeneralViewViewModel(viewItem.DisplayName))
            };

            return new List<TabItem> { generalTab };
        }

        private static List<TabItem> GenerateIndexTabs(IndexItem indexItem)
        {
            var generalTab = new TabItem
            {
                Header = LocalisationHelper.GetString("TabHeader_GeneralIndex", indexItem.DisplayName),
                Content = new IndexGeneralTabContent(new GeneralIndexViewModel(indexItem.DisplayName))
            };

            return new List<TabItem> { generalTab };
        }

        private static List<TabItem> GenerateTriggerTabs(TriggerItem triggerItem)
        {
            var generalTab = new TabItem
            {
                Header = LocalisationHelper.GetString("TabHeader_GeneralTrigger", triggerItem.DisplayName),
                Content = new TriggerGeneralTabContent(new GeneralTriggerViewModel(triggerItem.DisplayName))
            };

            return new List<TabItem> { generalTab };
        }
    }
}
