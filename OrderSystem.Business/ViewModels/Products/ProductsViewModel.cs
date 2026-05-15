using OrderSystem.Business.Classes;
using OrderSystem.Business.Orchestration.Interfaces;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using System.Collections.Specialized;
using System.Windows.Input;

namespace OrderSystem.Business.ViewModels.Products
{
    public class ProductsViewModel : ListViewModel<Product>
    {
        private readonly IProductOrchestrator _orchestrator;

        public ICommand ClearFilterCommand { get; }
        public SearchProvider ProductGroupsFilterSearchProvider { get; }
        public ProductsViewModel(IProductOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            AddCommand = new RelayCommand(() =>
            {
                if (_orchestrator.AddNewProduct(this) is Product newProduct)
                {
                    SelectedItem = newProduct;
                }
            });
            EditCommand = new RelayCommand<Product>(product => _orchestrator.EditProduct(product!, this), product => product is not null);
            DeleteCommand = new RelayCommand<Product>(product => _orchestrator.DeleteProduct(product!, this), product => product is not null);
            ProductGroupsFilterSearchProvider = new SearchProvider<ProductGroup?>(() => _orchestrator.SearchProductGroup(ProductGroupsFilterLookup, SelectedFilterProductGroup, this));
            ClearFilterCommand = new RelayCommand(ClearFilterCommandExecute);
            DataManager.Instance.Products.CollectionChanged += ProductsSourceChanged;
        }

        private void ClearFilterCommandExecute()
        {
            SelectedFilterProductGroup = null;
            SearchText = string.Empty;
        }

        public override void Dispose()
        {
            DataManager.Instance.Products.CollectionChanged -= ProductsSourceChanged;
            base.Dispose();
        }

        private void ProductsSourceChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(ProductsSource));
        }

        public IEnumerable<Product> ProductsSource
        {
            get
            {
                IEnumerable<Product > products = _orchestrator.ProductsSource;
                if (SelectedFilterProductGroup is not null)
                {
                    products = products.Where(product => ReferenceEquals(product.ProductGroup, SelectedFilterProductGroup));
                }
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    products = products.Where(product => product.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }
                return products;
            }
        }

        public override string ViewTitle => OrderResources.Products;

        public IEnumerable<ProductGroup> ProductGroupsFilterLookup =>
            DataManager.Instance.ProductGroups.Where(productGroup =>
            {
                if (!productGroup.IsExpired)
                    return true;
                return ProductsSource.Any(product => ReferenceEquals(product.ProductGroup, productGroup));
            });

        private ProductGroup? _selectedProductGroupFilter = null;
        public ProductGroup? SelectedFilterProductGroup
        {
            get => _selectedProductGroupFilter;
            set
            {
                if (_selectedProductGroupFilter != value)
                {
                    _selectedProductGroupFilter = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(ProductsSource));
                }
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(ProductsSource));
                }
            }
        }
    }
}
