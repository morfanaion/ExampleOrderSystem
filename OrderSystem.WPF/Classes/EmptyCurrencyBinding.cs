using OrderSystem.WPF.Converters;
using System.Windows;
using System.Windows.Data;

namespace OrderSystem.WPF.Classes
{
    internal class EmptyCurrencyBinding : Binding
    {
        public EmptyCurrencyBinding()
        {
            Converter = new PriceInCentsToStringConverter();
        }

        public EmptyCurrencyBinding(string path) : this()
        {
            Path = new PropertyPath(path);
        }
    }
}
