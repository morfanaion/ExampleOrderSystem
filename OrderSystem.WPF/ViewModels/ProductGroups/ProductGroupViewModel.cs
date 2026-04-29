using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;

namespace OrderSystem.WPF.ViewModels.ProductGroups
{
    internal class ProductGroupViewModel(ProductGroup productGroup, IValidator validator, string viewTitle) : ObjectViewModel<ProductGroup>(productGroup, validator)
    {
        public override string ViewTitle { get; } = viewTitle;
    }
}
