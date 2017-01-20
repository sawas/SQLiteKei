using log4net;

using SQLiteKei.Util;

using System.Collections.Generic;
using System;
using SQLiteKei.Commands;

namespace SQLiteKei.ViewModels.PreferencesWindow
{
    /// <summary>
    /// The ViewModel for the preference window
    /// </summary>
    public class PreferencesViewModel
    {
        private readonly ILog log = LogHelper.GetLogger();

        public List<string> AvailableLanguages { get; set; }

        private string selectedLanguage;
        public string SelectedLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(selectedLanguage))
                    selectedLanguage = GetLanguageFromSettings();

                return selectedLanguage;
            }
            set { selectedLanguage = value; }
        }

        private string GetLanguageFromSettings()
        {
            var setting = Properties.Settings.Default.UILanguage;

            switch(setting)
            {
                case "de-DE":
                    return LocalisationHelper.GetString("Preferences_Language_German");
                case "en-GB":
                default:
                    return LocalisationHelper.GetString("Preferences_Language_English");
            };
        }
        public PreferencesViewModel()
        {
            AvailableLanguages = new List<string>
            {
                LocalisationHelper.GetString("Preferences_Language_German"),
                LocalisationHelper.GetString("Preferences_Language_English")
            };

            applySettingsCommand = new DelegateCommand(ApplySettings);
        }

        private void ApplySettings()
        {
            ApplyLanguage();

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private readonly DelegateCommand applySettingsCommand;

        public DelegateCommand ApplySettingsCommand { get { return applySettingsCommand; } }

        private void ApplyLanguage()
        {
            if (selectedLanguage.Equals(GetLanguageFromSettings())) return;

            if (selectedLanguage.Equals(LocalisationHelper.GetString("Preferences_Language_German")))
            {
                Properties.Settings.Default.UILanguage = "de-DE";
            }
            else
            {
                Properties.Settings.Default.UILanguage = "en-GB";
            }

            log.Info("Applied application language " + Properties.Settings.Default.UILanguage);
        }
    }
}
