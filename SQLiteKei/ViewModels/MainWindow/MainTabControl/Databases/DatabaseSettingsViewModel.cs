using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.Properties;

namespace SQLiteKei.ViewModels.MainWindow.MainTabControl.Databases
{
    public class DatabaseSettingsViewModel
    {
        public short? SchemaVersion { get; set; }

        public short? UserVersion { get; set; }

        public int? ApplicationId { get; set; }

        public DatabaseSettingsViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            using (var dbHandler = new DatabaseHandler(Settings.Default.CurrentDatabase))
            {
                var settings = dbHandler.GetSettings();

                SchemaVersion = settings.SchemaVersion;
                UserVersion = settings.UserVersion;
                ApplicationId = settings.ApplicationId;
            }
        }

        private void Save()
        {
            var settings = new DbSettings
            {
                SchemaVersion = SchemaVersion,
                UserVersion = UserVersion,
                ApplicationId = ApplicationId
            };

            using (var dbHandler = new DatabaseHandler(Settings.Default.CurrentDatabase))
            {
                dbHandler.UpdateSetting
            }
        }

        private DelegateCommand saveCommand;

        public DelegateCommand SaveCommand { get { return saveCommand; } }
    }
}
