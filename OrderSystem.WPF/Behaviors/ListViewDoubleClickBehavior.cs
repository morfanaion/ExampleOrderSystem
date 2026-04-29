using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrderSystem.WPF.Behaviors
{
    public static class ListViewDoubleClickBehavior
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(ListViewDoubleClickBehavior),
                new PropertyMetadata(null, OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached(
                "CommandParameter",
                typeof(object),
                typeof(ListViewDoubleClickBehavior),
                new PropertyMetadata(null));

        public static void SetCommand(DependencyObject element, ICommand value)
            => element.SetValue(CommandProperty, value);

        public static ICommand GetCommand(DependencyObject element)
            => (ICommand)element.GetValue(CommandProperty);

        public static void SetCommandParameter(DependencyObject element, object value)
            => element.SetValue(CommandParameterProperty, value);

        public static object GetCommandParameter(DependencyObject element)
            => element.GetValue(CommandParameterProperty);

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListView listView)
            {
                listView.MouseDoubleClick -= OnMouseDoubleClick;

                if (e.NewValue != null)
                    listView.MouseDoubleClick += OnMouseDoubleClick;
            }
        }

        private static void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ListView listView)
                return;

            DependencyObject? original = e.OriginalSource as DependencyObject;

            if (ItemsControl.ContainerFromElement(listView, original) is not ListViewItem container)
                return;

            object item = container.DataContext;

            ICommand command = GetCommand(listView);

            object parameter = GetCommandParameter(listView) ?? item;

            if (command?.CanExecute(parameter) == true)
            {
                command.Execute(parameter);
            }
        }
    }
}
