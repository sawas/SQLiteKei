using log4net;

using SQLiteKei.ViewModels.Base;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.Properties;
using SQLiteKei.Util;

using System;
using System.Windows;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Indexes
{
    public class GeneralIndexViewModel : NotifyingModel
    {
        private ILog logger = LogHelper.GetLogger();

        private string indexName;
        public string IndexName
        {
            get { return indexName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        using (var indexHandler = new IndexHandler(Settings.Default.CurrentDatabase))
                        {
                            indexHandler.UpdateIndexName(indexName, value);
                            MainTreeHandler.UpdateIndexName(indexName, value, Settings.Default.CurrentDatabase);
                            indexName = value;
                            UpdateSql();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Failed to rename index '" + indexName + "' from view overview.", ex);
                        var message = LocalisationHelper.GetString("MessageBox_NameChangeWarning", ex.Message);

                        MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        public string TableName { get; set; }

        private bool isUnique;
        public bool IsUnique
        {
            get { return isUnique; }
            set
            {
                try
                {
                    using (var indexHandler = new IndexHandler(Settings.Default.CurrentDatabase))
                    {
                        indexHandler.UpdateIndexUniqueness(indexName, value);
                        isUnique = value;
                        UpdateSql();
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn("Failed to update index uniqueness enforcement setting on '" + indexName + "'.", ex);
                    var message = LocalisationHelper.GetString("MessageBox_IndexUniquenessWarning", ex.Message);

                    MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private string sqlStatement;
        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; NotifyPropertyChanged("SqlStatement"); }
        }

        public GeneralIndexViewModel(string indexName)
        {
            this.indexName = indexName;

            Initialize();
        }

        private void Initialize()
        {
            using (var indexHandler = new IndexHandler(Settings.Default.CurrentDatabase))
            {
                var index = indexHandler.GetIndex(indexName);

                TableName = index.Table;
                isUnique = index.IsUnique;
                sqlStatement = index.SqlStatement;
            }
        }

        private void UpdateSql()
        {
            using (var indexHandler = new IndexHandler(Settings.Default.CurrentDatabase))
            {
                var index = indexHandler.GetIndex(indexName);
                SqlStatement = index.SqlStatement;
            }
        }
    }
}
