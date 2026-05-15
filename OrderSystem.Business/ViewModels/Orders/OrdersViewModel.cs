using OrderSystem.Business.Classes;
using OrderSystem.Business.Orchestration.Interfaces;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using System.Collections.Specialized;
using System.Windows.Input;

namespace OrderSystem.Business.ViewModels.Orders
{
    public class OrdersViewModel : ListViewModel<Order>
    {
        private readonly IOrderOrchestrator _orchestrator;

        public ICommand ClearFilterCommand { get; }

        public SearchProvider ProductFilterSearchProvider { get; }

        public OrdersViewModel(IOrderOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            AddCommand = new RelayCommand(() =>
            {
                if (_orchestrator.AddNewOrder(this) is Order newOrder)
                {
                    SelectedItem = newOrder;
                }
            });
            EditCommand = new RelayCommand<Order>(order => _orchestrator.EditOrder(order!, this), order => order is not null);
            DeleteCommand = new RelayCommand<Order>(order => _orchestrator.DeleteOrder(order!, this), order => order is not null);
            ClearFilterCommand = new RelayCommand(ClearFilterCommandExecute);
            ProductFilterSearchProvider = new SearchProvider<Product?>(() => _orchestrator.SearchProduct(ProductFilterLookup, SelectedFilterProduct, this));
            DataManager.Instance.Orders.CollectionChanged += OrdersChanged;
        }

        private void OrdersChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(OrdersSource));
        }

        public override void Dispose()
        {
            DataManager.Instance.Orders.CollectionChanged -= OrdersChanged;
            base.Dispose();
        }

        private void ClearFilterCommandExecute()
        {
            FilterStartDate = null;
            FilterEndDate = null;
            SelectedFilterProduct = null;
        }

        public IEnumerable<Order> OrdersSource
        {
            get
            {
                IEnumerable<Order> source = _orchestrator.OrdersSource;
                if (FilterStartDate != null)
                {
                    source = source.Where(o => o.PlacedAt >= FilterStartDate.Value);
                }
                if (FilterEndDate != null)
                {
                    source = source.Where(o => o.PlacedAt <= FilterEndDate.Value);
                }
                if(SelectedFilterProduct is not null)
                {
                    source = source.Where(order => order.OrderLines.Any(line => ReferenceEquals(line.Product, SelectedFilterProduct)));
                }
                return source;
            }
        }

        public override string ViewTitle => OrderResources.Orders;

        private DateTime? _filterStartDate = null;
        public DateTime? FilterStartDate
        {
            get => _filterStartDate;
            set
            {
                if (_filterStartDate != value)
                {
                    _filterStartDate = value;
                    RaisePropertyChanged(nameof(OrdersSource));
                }
            }
        }

        private DateTime? _filterEndDate = null;
        public DateTime? FilterEndDate
        {
            get => _filterEndDate;
            set
            {
                if (_filterEndDate != value)
                {
                    _filterEndDate = value;
                    RaisePropertyChanged(nameof(OrdersSource));
                }
            }
        }

        public IEnumerable<Product> ProductFilterLookup => DataManager.Instance.Products;

        private Product? _selectedProductFilter = null;
        public Product? SelectedFilterProduct
        {
            get => _selectedProductFilter;
            set
            {
                if (_selectedProductFilter != value)
                {
                    _selectedProductFilter = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(OrdersSource));
                }
            }
        }
    }
}
