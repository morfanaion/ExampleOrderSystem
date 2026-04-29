using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace OrderSystem.WPF.Classes
{
    public sealed class CommitCommandWrapper : ICommand
    {
        private readonly ICommand _inner;
        private readonly WeakReference<DependencyObject> _rootRef;

        public CommitCommandWrapper(ICommand inner, DependencyObject root)
        {
            _inner = inner;
            _rootRef = new WeakReference<DependencyObject>(root);
        }

        public bool CanExecute(object? parameter) => _inner.CanExecute(parameter);

        public void Execute(object? parameter)
        {
            if (_rootRef.TryGetTarget(out var root))
            {
                CommitFocusedElement();
            }

            _inner.Execute(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add => _inner.CanExecuteChanged += value;
            remove => _inner.CanExecuteChanged -= value;
        }

        private static void CommitFocusedElement()
        {
            if (Keyboard.FocusedElement is not DependencyObject element)
                return;

            switch (element)
            {
                case TextBox tb:
                    tb.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
                    break;

                case ComboBox cb:
                    cb.GetBindingExpression(Selector.SelectedItemProperty)?.UpdateSource();
                    break;

                case DatePicker dp:
                    dp.GetBindingExpression(DatePicker.SelectedDateProperty)?.UpdateSource();
                    break;
            }
        }
    }
}
