using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OrderSystem.WPF.Converters
{
    public class EmptyStringToIntConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value.ToString();

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? s = value as string;

            if (string.IsNullOrWhiteSpace(s))
                return 0;

            if (int.TryParse(s, out var result))
                return result;

            return DependencyProperty.UnsetValue;
        }
    }
}
