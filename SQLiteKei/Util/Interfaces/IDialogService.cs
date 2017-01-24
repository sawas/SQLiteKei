namespace SQLiteKei.Util.Interfaces
{
    internal interface IDialogService
    {
        bool AskForUserAgreement(string messageBodyKey, string messageTitleKey, string itemName);

        void ShowMessage(string messageBodyKey);
    }
}
