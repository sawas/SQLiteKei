using System;
using System.Windows.Input;

namespace SQLiteKei.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action action;

        private readonly Predicate<object> canExecute; 

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action action, Predicate<object> canExecute = null)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            action();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
