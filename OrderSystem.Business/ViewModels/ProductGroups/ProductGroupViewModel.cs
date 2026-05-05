using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;

namespace OrderSystem.Business.ViewModels.ProductGroups
{
    public class ProductGroupViewModel(ProductGroup productGroup, IValidator validator, string viewTitle) : ObjectViewModel<ProductGroup>(productGroup, validator)
    {
        public override string ViewTitle { get; } = viewTitle;
    }
}
