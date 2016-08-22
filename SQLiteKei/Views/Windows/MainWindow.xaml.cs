using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.ElementRenameWindow;
using SQLiteKei.ViewModels.MainWindow;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using SQLiteKei.ViewModels.TableMigrator;
using SQLiteKei.Views.Windows.Creators;

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace SQLiteKei.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel viewModel;

        private readonly ILog log = LogHelper.GetLogger();

        public MainWindow()
        {
            viewModel = new MainWindowViewModel(new TreeSaveHelper());
            DataContext = viewModel;

            InitializeComponent();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void OpenPreferences(object sender, RoutedEventArgs e)
        {
            new Preferences().ShowDialog();
        }

        private void OpenQueryEditor(object sender, RoutedEventArgs e)
        {
            new QueryEditor(viewModel.TreeViewItems).ShowDialog();
        }

        private void OpenTableCreator(object sender, RoutedEventArgs e)
        {
            new TableCreator(viewModel.TreeViewItems).ShowDialog();
        }

        private void OpenViewCreator(object sender, RoutedEventArgs e)
        {
            new ViewCreator(viewModel.TreeViewItems).ShowDialog();
        }

        private void OpenIndexCreator(object sender, RoutedEventArgs e)
        {
            new IndexCreator(viewModel.TreeViewItems).ShowDialog();
        }

        private void OpenTriggerCreator(object sender, RoutedEventArgs e)
        {
            new TriggerCreator(viewModel.TreeViewItems).ShowDialog();
        }

        private void CreateNewDatabase(object sender, RoutedEventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "SQLite (*.sqlite)|*.sqlite";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DatabaseHandler.CreateDatabase(dialog.FileName);
                    viewModel.OpenDatabase(dialog.FileName);
                }
            }
        }

        private void CloseDatabase(object sender, RoutedEventArgs e)
        {
            var selectedItem = DBTreeView.SelectedItem as TreeItem;

            if (selectedItem != null)
                viewModel.CloseDatabase(selectedItem.DatabasePath);
        }

        private void OpenDatabase(object sender, RoutedEventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Database Files (*.sqlite, *.db)|*.sqlite; *db; |All Files |*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    viewModel.OpenDatabase(dialog.FileName);
                }
            }
        }

        private void DeleteDatabase(object sender, RoutedEventArgs e)
        {
            var selectedItem = DBTreeView.SelectedItem as DatabaseItem;

            if (selectedItem != null)
            {
                var message = LocalisationHelper.GetString("MessageBox_DatabaseDeleteWarning", selectedItem.DisplayName);
                var result = System.Windows.MessageBox.Show(message, LocalisationHelper.GetString("MessageBoxTitle_DatabaseDeletion"), MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes) return;
                if (!File.Exists(selectedItem.DatabasePath))
                    throw new FileNotFoundException("Database file could not be found.");

                File.Delete(selectedItem.DatabasePath);
                viewModel.CloseDatabase(selectedItem.DatabasePath);
            }
        }

        private void RefreshTree(object sender, RoutedEventArgs e)
        {
            viewModel.RefreshTree();
        }

        private void DBTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetGlobalDatabaseString();
            ResetTabControl();

            var currentSelection = (TreeItem)DBTreeView.SelectedItem;
            viewModel.SelectedItem = currentSelection;
            var tabs = DatabaseTabGenerator.GenerateTabsFor(currentSelection);

            foreach (TabItem tab in tabs)
                MainTabControl.Items.Add(tab);
        }

        private void SetGlobalDatabaseString()
        {
            if(DBTreeView.SelectedItem != null)
            {
                var currentSelection = (TreeItem)DBTreeView.SelectedItem;
                Properties.Settings.Default.CurrentDatabase = currentSelection.DatabasePath;
            }
        }

        private void ResetTabControl()
        {
            var openTabs = MainTabControl.Items.Count;

            for (var i = openTabs-1; i >= 0; i--)
                MainTabControl.Items.RemoveAt(i);

            var defaultTabs = DatabaseTabGenerator.GenerateDefaultTabs();

            foreach (TabItem tab in defaultTabs)
                MainTabControl.Items.Add(tab);

            MainTabControl.SelectedIndex = 0;
        }

        private void EmptyTable(object sender, RoutedEventArgs e)
        {
            var tableItem = (TableItem)DBTreeView.SelectedItem;
            viewModel.EmptyTable(tableItem.DisplayName);
        }

        private void MoveTable(object sender, RoutedEventArgs e)
        {
            var treeItem = DBTreeView.SelectedItem as TableItem;
            var tableMigratorViewModel = new TableMigratorViewModel(viewModel.TreeViewItems, treeItem.DisplayName, MigrationType.Move);

            new TableMigrator(tableMigratorViewModel).ShowDialog();
        }

        private void CopyTable(object sender, RoutedEventArgs e)
        {
            var treeItem = DBTreeView.SelectedItem as TableItem;
            var tableMigratorViewModel = new TableMigratorViewModel(viewModel.TreeViewItems, treeItem.DisplayName, MigrationType.Copy);

            new TableMigrator(tableMigratorViewModel).ShowDialog();
        }

        private void DeleteTable(object sender, RoutedEventArgs e)
        {
            var tableItem = (TableItem)DBTreeView.SelectedItem;
            viewModel.DeleteTable(tableItem);
        }

        private void DeleteView(object sender, RoutedEventArgs e)
        {
            var viewItem = (ViewItem)DBTreeView.SelectedItem;
            viewModel.DeleteView(viewItem);
        }

        private void DeleteIndex(object sender, RoutedEventArgs e)
        {
            var viewItem = (IndexItem)DBTreeView.SelectedItem;
            viewModel.DeleteIndex(viewItem);
        }

        private void DeleteTrigger(object sender, RoutedEventArgs e)
        {
            var triggerItem = (TriggerItem)DBTreeView.SelectedItem;
            viewModel.DeleteTrigger(triggerItem);
        }

        private void Rename(object sender, RoutedEventArgs e)
        {
            var selectedItem = DBTreeView.SelectedItem as TreeItem;
            new ElementRenameWindow(new ElementRenameViewModel(selectedItem)).ShowDialog();            
        }

        protected override void OnClosed(EventArgs e)
        {
            viewModel.SaveTree();
        }        

        private void OpenFileDirectory(object sender, RoutedEventArgs e)
        {
            var database = (DatabaseItem)DBTreeView.SelectedItem;
            var targetDirectory = Path.GetDirectoryName(database.DatabasePath);

            Process.Start(targetDirectory);
        }

        #region TreeViewRightClickEvent
        /// <summary>
        /// Method that is used to make sure a tree view element is selected on a right click event before the context menu is opened.
        /// </summary>
        private void TreeViewRightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
        #endregion
    }
}
