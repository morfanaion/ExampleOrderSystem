using System.Windows.Input;

namespace OrderSystem.WPF.Classes
{
    internal class RelayCommand(Action execute, Func<bool>? canExecute) : ICommand
    {
        private readonly Action _execute = execute;
        private readonly Func<bool>? _canExecute = canExecute;

        public RelayCommand(Action execute) : this(execute, null)
        {
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    internal class RelayCommand<T>(Action<T?> execute, Func<T?, bool>? canExecute) : ICommand
    {
        private readonly Action<T?> _execute = execute;
        private readonly Func<T?, bool>? _canExecute = canExecute;

        public RelayCommand(Action<T?> execute) : this(execute, null)
        {
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute?.Invoke((T?)parameter) ?? true;

        public void Execute(object? parameter) => _execute((T?)parameter);

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
