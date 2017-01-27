﻿using SQLiteKei.ViewModels.Base;

using System.Collections.ObjectModel;

namespace SQLiteKei.ViewModels.SelectQueryWindow
{
    public class OrderItem : NotifyingModel
    {
        public ObservableCollection<string> Columns { get; set; }

        private string selectedColumn;
        public string SelectedColumn
        {
            get { return selectedColumn; }
            set { selectedColumn = value; NotifyPropertyChanged(); }
        }

        private bool isDescending;
        public bool IsDescending
        {
            get { return isDescending; }
            set { isDescending = value; NotifyPropertyChanged(); }
        }

        public OrderItem()
        {
            Columns = new ObservableCollection<string>();
        }
    }
}
