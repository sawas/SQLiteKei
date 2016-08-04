using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System;


namespace SQLiteKei.ViewModels.MainWindow.DBTreeView
{
    // TODO replace with FolderItems for Triggers, Views and Indexes lateron
    [Obsolete("Needs to be replaced with concrete FolderItems for each type so they can have individual context menu actions.")]
    public class FolderItem : DirectoryItem
    {
    }
}
