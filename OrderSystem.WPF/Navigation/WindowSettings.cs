using System.Windows;

namespace OrderSystem.WPF.Navigation
{
    public class WindowSettings
    {
        public static readonly DependencyProperty SettingsProperty = DependencyProperty.RegisterAttached("Settings", typeof(WindowSettings), typeof(WindowSettings));

        public static void SetSettings(DependencyObject obj, WindowSettings value) => obj.SetValue(SettingsProperty, value);

        public static WindowSettings? GetSettings(DependencyObject obj) => (WindowSettings?)obj.GetValue(SettingsProperty);

        public ResizeMode? ResizeMode { get; set; }
        public WindowStyle? WindowStyle { get; set; }
        public double? MinWidth { get; set; }
        public double? MaxWidth { get; set; }
        public double? Width { get; set; }
        public double? MinHeight { get; set; }
        public double? MaxHeight { get; set; }
        public double? Height { get; set; }
    }
}
