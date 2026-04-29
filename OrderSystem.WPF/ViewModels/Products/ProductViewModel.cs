using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace OrderSystem.WPF.ViewModels.Products
{
    internal class ProductViewModel : ObjectViewModel<Product>
    {
        public override string ViewTitle { get; }

        public ProductViewModel(Product product, IValidator validator, string viewTitle) : base(product, validator)
        {
            ViewTitle = viewTitle;
            ProductGroupsLookup = CollectionViewSource.GetDefaultView(DataManager.Instance.ProductGroups);
            ProductGroupsLookup.Filter = item =>
            {
                if (item is ProductGroup productGroup)
                {
                    if (!productGroup.IsExpired)
                    {
                        return true;
                    }
                    return ReferenceEquals(CurrentObject.ProductGroup, productGroup);
                }
                return false;
            };
            DataManager.Instance.ProductGroups.CollectionChanged += ProductGroupsChanged;
            product.PropertyChanged += Product_PropertyChanged;
        }

        private void Product_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Product.ProductGroupId))
            {
                ProductGroupsLookup.Refresh();
            }
        }

        public override void Dispose()
        {
            DataManager.Instance.ProductGroups.CollectionChanged -= ProductGroupsChanged;
            CurrentObject.PropertyChanged -= Product_PropertyChanged;
            base.Dispose();
        }

        private void ProductGroupsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ProductGroupsLookup.Refresh();
        }

        public ICollectionView ProductGroupsLookup { get; }
    }
}
