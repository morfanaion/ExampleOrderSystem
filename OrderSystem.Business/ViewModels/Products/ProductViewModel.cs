using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using System.Collections.Specialized;
using System.ComponentModel;

namespace OrderSystem.Business.ViewModels.Products
{
    public class ProductViewModel : ObjectViewModel<Product>
    {
        public override string ViewTitle { get; }

        public ProductViewModel(Product product, IValidator validator, string viewTitle) : base(product, validator)
        {
            ViewTitle = viewTitle;
            DataManager.Instance.ProductGroups.CollectionChanged += ProductGroupsChanged;
            product.PropertyChanged += Product_PropertyChanged;
        }

        private void Product_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Product.ProductGroupId))
            {
                RaisePropertyChanged(nameof(ProductGroupsLookup));
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
            RaisePropertyChanged(nameof(ProductGroupsLookup));
        }

        public IEnumerable<ProductGroup> ProductGroupsLookup =>
            DataManager.Instance.ProductGroups.Where(productGroup =>
            {
                if (!productGroup.IsExpired)
                    return true;

                return ReferenceEquals(CurrentObject.ProductGroup, productGroup);
            });
    }
}
