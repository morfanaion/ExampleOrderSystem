using Microsoft.Xaml.Behaviors;
using OrderSystem.WPF.Navigation;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OrderSystem.WPF.Behaviors
{
    public class SyncWindowSizeBehavior : Behavior<ContentControl>
    {
        private DependencyPropertyDescriptor? _dpd;

        protected override void OnAttached()
        {
            base.OnAttached();

            _dpd = DependencyPropertyDescriptor
            .FromProperty(ContentControl.ContentProperty, typeof(ContentControl));

            _dpd.AddValueChanged(AssociatedObject, OnContentChanged);
            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            Cleanup();
            base.OnDetaching();
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            if(_dpd != null)
            {
                _dpd.RemoveValueChanged(AssociatedObject, OnContentChanged);
                _dpd = null;
            }

            if (AssociatedObject != null)
            {
                AssociatedObject.Unloaded -= OnUnloaded;
            }
        }

        private void OnContentChanged(object? sender, EventArgs e)
        {
            AssociatedObject.Dispatcher.InvokeAsync(() =>
            {
                ContentPresenter? presenter = FindChild<ContentPresenter>(AssociatedObject);
                if (presenter == null)
                {
                    return;
                }

                presenter.ApplyTemplate();

                FrameworkElement? view = VisualTreeHelper.GetChildrenCount(presenter) > 0
                    ? VisualTreeHelper.GetChild(presenter, 0) as FrameworkElement
                    : null;

                if (view == null)
                    return;

                WindowSettings? settings = WindowSettings.GetSettings(view);
                if ((settings == null))
                {
                    return;
                }
                
                var window = Window.GetWindow(view);
                if (window == null) return;

                if(settings.MinWidth.HasValue)
                {
                    window.MinWidth = settings.MinWidth.Value;
                }
                if (settings.MaxWidth.HasValue)
                {
                    window.MaxWidth = settings.MaxWidth.Value;
                }
                if (settings.Width.HasValue)
                {
                    window.Width = settings.Width.Value;
                }
                if (settings.MinHeight.HasValue)
                {
                    window.MinHeight = settings.MinHeight.Value;
                }
                if (settings.MaxHeight.HasValue)
                {
                    window.MaxHeight = settings.MaxHeight.Value;
                }
                if (settings.Height.HasValue)
                {
                    window.Height = settings.Height.Value;
                }
                if (settings.ResizeMode.HasValue)
                {
                    window.ResizeMode = settings.ResizeMode.Value;
                }
                if (settings.WindowStyle.HasValue)
                {
                    window.WindowStyle = settings.WindowStyle.Value;
                }
            });
        }

        private static T? FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild)
                    return typedChild;

                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
