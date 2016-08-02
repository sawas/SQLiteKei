using log4net;

using SQLiteKei.Commands;
using SQLiteKei.Util;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SQLiteKei.ViewModels.QueryEditorWindow
{
    public class QueryEditorViewModel : NotifyingModel
    {
        private ILog logger = LogHelper.GetLogger();

        public List<DatabaseSelectItem> Databases { get; set; }

        private string sqlStatement;
        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; NotifyPropertyChanged("SqlStatement"); }
        }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        public QueryEditorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();
            sqlStatement = string.Empty;

            saveCommand = new DelegateCommand(SaveQuery);
            loadCommand = new DelegateCommand(LoadQuery);
        }

        private void SaveQuery()
        {
            using (var fileDialog = new SaveFileDialog())
            {
                fileDialog.Filter = "SQL Files(*.sql)|*.sql";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(fileDialog.FileName, sqlStatement);
                        logger.Info("Loaded query to file.");
                    }
                    catch (Exception ex)
                    {
                        logger.Info("Could not save query to file " + fileDialog.FileName, ex);
                        StatusInfo = LocalisationHelper.GetString("QueryEditor_FileSaveFailed");
                    }
                }
            }
        }

        private readonly DelegateCommand saveCommand;

        public DelegateCommand SaveCommand { get { return saveCommand; } }

        private void LoadQuery()
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "SQL Files(*.sql)|*.sql; |All Files |*.*";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        SqlStatement = File.ReadAllText(fileDialog.FileName);
                        logger.Info("Loaded query from file.");
                    }
                    catch (Exception ex)
                    {
                        logger.Info("Could not read query from file " + fileDialog.FileName, ex);
                        StatusInfo = LocalisationHelper.GetString("QueryEditor_FileLoadFailed");
                    }
                }
            }
        }

        private readonly DelegateCommand loadCommand;

        public DelegateCommand LoadCommand { get { return loadCommand; } }
    }
}
