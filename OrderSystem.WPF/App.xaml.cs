using OrderSystem.WPF.Orchestration;
using OrderSystem.WPF.ResourceDictionaries;
using OrderSystem.WPF.Services;
using OrderSystem.WPF.ViewModels;
using OrderSystem.WPF.ViewModels.Orders;
using OrderSystem.WPF.ViewModels.ProductGroups;
using OrderSystem.WPF.ViewModels.Products;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace OrderSystem.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            Application.Current.Resources.MergedDictionaries.Add(new OrdersDictionary());
            Application.Current.Resources.MergedDictionaries.Add(new OrderLinesDictionary());
            Application.Current.Resources.MergedDictionaries.Add(new ProductsDictionary());
            Application.Current.Resources.MergedDictionaries.Add(new ProductGroupsDictionary());
            base.OnStartup(e);

            ServiceLocator.RegisterService<IUiService, UiService>(new UiService());

            List<ViewModel> tabs = [
                new OrdersViewModel(new OrderOrchestrator()),
                new ProductGroupsViewModel(new ProductGroupOrchestrator()),
                new ProductsViewModel(new ProductOrchestrator())
                ];

            MainViewModel viewModel = new MainViewModel(tabs);

            ServiceLocator.GetService<IUiService>().Show(viewModel);
        }
    }

}
