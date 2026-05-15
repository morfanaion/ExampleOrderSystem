using OrderSystem.Business.Classes;
using OrderSystem.Business.Orchestration.Interfaces;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace OrderSystem.Business.ViewModels.OrderLines
{
    public class OrderLineViewModel : ObjectViewModel<OrderLine>
    {
        private IOrderlineOrchestrator _orchestrator;
        public override string ViewTitle { get; }

        public SearchProvider ProductSearchProvider { get; }

        public OrderLineViewModel(OrderLine orderLine, IOrderlineOrchestrator orchestrator, IValidator validator, string viewTitle) : base(orderLine, validator)
        {
            _orchestrator = orchestrator;
            ViewTitle = viewTitle;
            DataManager.Instance.Products.CollectionChanged += ProductsChanged;
            DataManager.Instance.ProductGroups.CollectionChanged += ProductsChanged;
            CurrentObject.PropertyChanged += CurrentObject_PropertyChanged;
            ProductSearchProvider = new SearchProvider<Product?>(() => _orchestrator.SearchProduct(ProductsLookup, CurrentObject.Product, this));
        }

        private void CurrentObject_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OrderLine.ProductId):
                    RaisePropertyChanged(nameof(ProductsLookup));
                    break;
            }
        }

        public override void Dispose()
        {
            DataManager.Instance.Products.CollectionChanged -= ProductsChanged;
            DataManager.Instance.ProductGroups.CollectionChanged -= ProductsChanged;
            base.Dispose();
        }

        private void ProductsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(ProductsLookup));
        }



        public IEnumerable<Product> ProductsLookup =>
            DataManager.Instance.Products.Where(product =>
            {
                if (!product.IsExpired && !product.ProductGroup!.IsExpired)
                    return true;

                return ReferenceEquals(CurrentObject.Product, product);
            });
    }
}
