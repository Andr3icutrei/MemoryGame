using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoryGame.Model
{
    public class RelayCommand<T> : ICommand
    {
        #region Private Fields
        private readonly Action<T> _execute;
        private readonly Predicate<T>? _canExecute;
        #endregion
        #region Constructor
        public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        #endregion

        #region Methods
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || parameter is T t && _canExecute(t);
        }

        public void Execute(object? parameter)
        {
            if (parameter is T t)
                _execute(t);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion
    }
}
