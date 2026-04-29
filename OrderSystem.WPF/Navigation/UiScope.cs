using System.Windows;

namespace OrderSystem.WPF.Navigation
{
    internal class UiScope(Window rootWindow)
    {
        public static readonly DependencyProperty UiScopeProperty =
        DependencyProperty.RegisterAttached(
            "Scope",
            typeof(UiScope),
            typeof(UiScope),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetScope(DependencyObject obj, UiScope value)
            => obj.SetValue(UiScopeProperty, value);

        public static UiScope GetScope(DependencyObject obj)
            => (UiScope)obj.GetValue(UiScopeProperty);

        public Window RootWindow { get; } = rootWindow;
    }
}
