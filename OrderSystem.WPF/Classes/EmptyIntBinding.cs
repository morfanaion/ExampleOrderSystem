using OrderSystem.WPF.Converters;
using System.Windows;
using System.Windows.Data;

namespace OrderSystem.WPF.Classes
{
    internal class EmptyIntBinding : Binding
    {
        public EmptyIntBinding()
        {
            Converter = new EmptyStringToIntConverter();
        }

        public EmptyIntBinding(string path) : this()
        {
            Path = new PropertyPath(path);
        }
    }
}
