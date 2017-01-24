using log4net;

using SQLiteKei.ViewModels.Base;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.Properties;
using SQLiteKei.Util;
using SQLiteKei.Util.Interfaces;

using System;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Indexes
{
    public class GeneralIndexViewModel : NotifyingModel
    {
        private ILog logger = LogHelper.GetLogger();

        private readonly IDialogService dialogService = new DialogService();

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
                        dialogService.ShowMessage("MessageBox_NameChangeFailed");
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
                    dialogService.ShowMessage("MessageBox_IndexUniquenessWarning");
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
