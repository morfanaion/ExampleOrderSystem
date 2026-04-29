using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using OrderSystem.WPF.Classes;
using OrderSystem.WPF.Orchestration.Interfaces;
using System.Collections.Specialized;

namespace OrderSystem.WPF.ViewModels.Products
{
    internal class ProductsViewModel : ViewModel
    {
        private readonly IProductOrchestrator _orchestrator;

        public RelayCommand AddCommand { get; }
        public RelayCommand<Product> EditCommand { get; }
        public RelayCommand<Product> DeleteCommand { get; }

        public ProductsViewModel(IProductOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            AddCommand = new RelayCommand(() => _orchestrator.AddNewProduct(this));
            EditCommand = new RelayCommand<Product>(product => _orchestrator.EditProduct(product!, this), product => product is not null);
            DeleteCommand = new RelayCommand<Product>(product => _orchestrator.DeleteProduct(product!, this), product => product is not null);
            if (_orchestrator.ProductsSource is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += ProductsSourceChanged;
            }
        }

        public override void Dispose()
        {
            if(_orchestrator.ProductsSource is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged -= ProductsSourceChanged;
            }
            base.Dispose();
        }

        private void ProductsSourceChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(ProductsSource));
        }

        public IEnumerable<Product> ProductsSource => _orchestrator.ProductsSource.Where(p => !p.IsExpired && !p.ProductGroup!.IsExpired);

        public override string ViewTitle => OrderResources.Products;
    }
}
