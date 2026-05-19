using OrderSystem.Business.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrderSystem.WPF.Controls
{
    public class SearchComboBox : ComboBox
    {
        public static DependencyProperty SearchProviderProperty = DependencyProperty.Register(
            nameof(SearchProvider), 
            typeof(SearchProvider), 
            typeof(SearchComboBox), 
            new PropertyMetadata(null));

        public SearchProvider SearchProvider
        {
            get => (SearchProvider)GetValue(SearchProviderProperty);
            set => SetValue(SearchProviderProperty, value);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if(SearchProvider is not null)
            {
                SelectedItem = SearchProvider.Search();
            }
            base.OnMouseDoubleClick(e);
        }
    }
}
