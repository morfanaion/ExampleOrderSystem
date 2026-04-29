using System.Globalization;
using System.Windows.Data;

namespace OrderSystem.WPF.Converters
{
    public class PriceInCentsToStringConverter : IValueConverter
    {
        public bool IncludeCurrencySign { get; set; }
        public bool AllowNegative
        {
            set
            {
                _firstIndexSearchString = value ? "-012345679" : "012345679";
            }
        }
        private string _firstIndexSearchString = "012345679";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is not int priceInCents)
            {
                return string.Empty;
            }
            return (priceInCents / 100m).ToString(IncludeCurrencySign ? "C" : "N");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is not string currencyString)
            {
                return 0;
            }
            int startIdx = currencyString.IndexOfAny(_firstIndexSearchString.ToCharArray());
            if (startIdx == -1)
            {
                return 0;
            }
            currencyString = currencyString[startIdx..];
            string[] parts = currencyString.Split(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            if (!int.TryParse(parts[0], out int wholeUnits))
            {
                return 0;
            }
            int cents = 0;
            if (parts.Length > 1)
            {
                string secondPart = parts[1];
                switch(secondPart.Length)
                {
                    case 0:
                        break;
                    case 1:
                        if (!int.TryParse(secondPart, out cents))
                        {
                            return 0;
                        }
                        cents *= 10;
                        break;
                    case 2:
                        if (!int.TryParse(secondPart, out cents))
                        {
                            return 0;
                        }
                        break;
                    default:
                        if (!int.TryParse(secondPart[..2], out cents))
                        {
                            return 0;
                        }
                        break;

                }
            }
            return wholeUnits * 100 + cents;
        }
    }
}
