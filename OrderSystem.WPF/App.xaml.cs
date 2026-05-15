using OrderSystem.Business.Orchestration;
using OrderSystem.Business.Services;
using OrderSystem.Business.ViewModels;
using OrderSystem.Business.ViewModels.Orders;
using OrderSystem.Business.ViewModels.ProductGroups;
using OrderSystem.Business.ViewModels.Products;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Resources;
using OrderSystem.WPF.ResourceDictionaries;
using OrderSystem.WPF.Services;
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
                new ProductGroupsViewModel(new ProductGroupOrchestrator(DataManager.Instance.ProductGroups.Where(pg => !pg.IsExpired))),
                new ProductsViewModel(new ProductOrchestrator(DataManager.Instance.Products.Where(p => !p.IsExpired && !p.ProductGroup!.IsExpired)))
                ];

            MainViewModel viewModel = new MainViewModel(tabs);

            ServiceLocator.GetService<IUiService>().Show(viewModel);
        }
    }

}
