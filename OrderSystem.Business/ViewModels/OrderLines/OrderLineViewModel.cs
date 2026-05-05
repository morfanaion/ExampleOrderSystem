using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using System.Collections.Specialized;
using System.ComponentModel;

namespace OrderSystem.Business.ViewModels.OrderLines
{
    public class OrderLineViewModel : ObjectViewModel<OrderLine>
    {
        public override string ViewTitle { get; }

        public OrderLineViewModel(OrderLine orderLine, IValidator validator, string viewTitle) : base(orderLine, validator)
        {
            ViewTitle = viewTitle;
            DataManager.Instance.Products.CollectionChanged += ProductsChanged;
            DataManager.Instance.ProductGroups.CollectionChanged += ProductsChanged;
            CurrentObject.PropertyChanged += CurrentObject_PropertyChanged;
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
