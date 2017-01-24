using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Properties;
using SQLiteKei.Util;
using SQLiteKei.Util.Interfaces;
using SQLiteKei.ViewModels.Base;

using System;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Trigger
{
    public class GeneralTriggerViewModel : NotifyingModel
    {
        private ILog logger = LogHelper.GetLogger();

        private IDialogService dialogService = new DialogService();

        private string triggerName;
        public string TriggerName
        {
            get { return triggerName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        using (var triggerHandler = new TriggerHandler(Settings.Default.CurrentDatabase))
                        {
                            triggerHandler.UpdateTriggerName(triggerName, value);
                            MainTreeHandler.UpdateTriggerName(triggerName, value, Settings.Default.CurrentDatabase);
                            triggerName = value;
                            UpdateSQL();
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Failed to rename trigger '" + triggerName + "' from trigger overview.", ex);
                        dialogService.ShowMessage("MessageBox_NameChangeFailed");
                    }
                }
            }
        }

        public string Target { get; set; }

        private string sqlStatement { get; set; }
        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; NotifyPropertyChanged("SqlStatement"); }
        }

        private string statusInfo { get; set; }
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        public GeneralTriggerViewModel(string triggerName)
        {
            this.triggerName = triggerName;

            Initialize();
        }

        private void Initialize()
        {
            using (var triggerHandler = new TriggerHandler(Settings.Default.CurrentDatabase))
            {
                var trigger = triggerHandler.GetTrigger(triggerName);

                Target = trigger.Target;
                sqlStatement = trigger.SqlStatement;
            }
        }

        private void UpdateSQL()
        {
            using (var triggerHandler = new TriggerHandler(Settings.Default.CurrentDatabase))
            {
                var trigger = triggerHandler.GetTrigger(triggerName);

                SqlStatement = trigger.SqlStatement;
            }
        }
    }
}
