using OrderSystem.Business.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OrderSystem.WPF.Navigation
{
    public static class ViewTrackingBehavior
    {
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached(
                "Enable",
                typeof(bool),
                typeof(ViewTrackingBehavior),
                new PropertyMetadata(false, OnEnableChanged));

        public static void SetEnable(DependencyObject obj, bool value) => obj.SetValue(EnableProperty, value);
        public static bool GetEnable(DependencyObject obj) => (bool)obj.GetValue(EnableProperty);

        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ContentPresenter cp && (bool)e.NewValue)
            {
                cp.Loaded += OnLoaded;
            }
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not ContentPresenter cp)
            {
                return;
            }

            if(VisualTreeHelper.GetChildrenCount(cp) == 0)
            {
                return;
            }

            if(VisualTreeHelper.GetChild(cp, 0) is not FrameworkElement view)
            {
                return;
            }

            if(view.DataContext is not ViewModel vm)
            {
                return;
            }

            HostRegistry.Register(vm, view);

            cp.Loaded -= OnLoaded;

        }
    }
}
