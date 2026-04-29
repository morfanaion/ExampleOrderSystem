using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrderSystem.WPF.Classes
{
    public static class CommandExtensions
    {
        public static readonly DependencyProperty CommitBeforeExecuteProperty =
            DependencyProperty.RegisterAttached(
                "CommitBeforeExecute",
                typeof(bool),
                typeof(CommandExtensions),
                new PropertyMetadata(false, OnChanged));

        public static void SetCommitBeforeExecute(DependencyObject element, bool value)
            => element.SetValue(CommitBeforeExecuteProperty, value);

        public static bool GetCommitBeforeExecute(DependencyObject element)
            => (bool)element.GetValue(CommitBeforeExecuteProperty);

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Button button)
                return;

            if (e.NewValue is true)
            {
                button.Loaded += (_, __) =>
                {
                    if (button.Command is ICommand original)
                    {
                        button.Command = new CommitCommandWrapper(original, button);
                    }
                };
            }
        }
    }
}
