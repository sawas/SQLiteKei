using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SQLiteKei.ViewModels.Base
{
    /// <summary>
    /// An object that implements the INotifyPropertyChanged interface
    /// </summary>
    public abstract class NotifyingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName]string property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
