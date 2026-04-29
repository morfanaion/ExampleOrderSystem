using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace OrderSystem.WPF.ViewModels.OrderLines
{
    internal class OrderLineViewModel : ObjectViewModel<OrderLine>
    {
        public override string ViewTitle { get; }

        public OrderLineViewModel(OrderLine orderLine, IValidator validator, string viewTitle) : base(orderLine, validator)
        {
            ViewTitle = viewTitle;
            DataManager.Instance.Products.CollectionChanged += ProductsChanged;
            DataManager.Instance.ProductGroups.CollectionChanged += ProductsChanged;
            CurrentObject.PropertyChanged += CurrentObject_PropertyChanged;
            ProductsLookup = CollectionViewSource.GetDefaultView(DataManager.Instance.Products);
            ProductsLookup.Filter = item =>
            {
                if (item is Product product)
                {
                    if(!product.IsExpired && !product.ProductGroup!.IsExpired)
                    {
                        return true;
                    }
                    return ReferenceEquals(CurrentObject.Product, product);
                }
                return false;
            };
        }

        private void CurrentObject_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OrderLine.ProductId):
                    ProductsLookup.Refresh();
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
            ProductsLookup.Refresh();
        }

        public ICollectionView ProductsLookup { get; }
    }
}
