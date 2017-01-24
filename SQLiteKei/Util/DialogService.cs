using SQLiteKei.Util.Interfaces;
using System.Windows;

namespace SQLiteKei.Util
{
    internal class DialogService : IDialogService
    {
        public bool AskForUserAgreement(string messageBodyKey, string messageTitleKey, string itemName)
        {
            var fullMessage = LocalisationHelper.GetString(messageBodyKey, itemName);
            var result = MessageBox.Show(fullMessage, LocalisationHelper.GetString(messageTitleKey), MessageBoxButton.YesNo, MessageBoxImage.Warning);

            return result == MessageBoxResult.Yes;
        }

        public void ShowMessage(string messageBodyKey)
        {
            var errorMessage = LocalisationHelper.GetString(messageBodyKey);

            MessageBox.Show(errorMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
