using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;

namespace OrderSystem.Data.Validation
{
    public class ProductValidator : IValidator
    {
        public ValidationState this[string propertyName] => _validationDictionary[propertyName];
        private Product _product;
        private Dictionary<string, ValidationState> _validationDictionary;

        public ProductValidator(Product product)
        {
            ValidationState productgroupState = new ValidationState() { Level = ValidationLevel.Valid };

            _validationDictionary = new Dictionary<string, ValidationState>()
            {
                { nameof(Product.ProductGroup), productgroupState },
                { nameof(Product.ProductGroupId), productgroupState },
                { nameof(Product.Name), new ValidationState() {Level = ValidationLevel.Valid} },
                { nameof(Product.PriceInCents), new ValidationState() {Level = ValidationLevel.Valid} },
            };
            _product = product;
        }

        public ValidationLevel Validate()
        {
            ValidationState state = _validationDictionary[nameof(Product.ProductGroup)];
            if (string.IsNullOrEmpty(_product.ProductGroup?.Id))
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.ProductGroupCannotBeEmpty;
            }
            else
            {
                state.Level = ValidationLevel.Valid;
                state.Message = string.Empty;
            }
            
            state = _validationDictionary[nameof(Product.Name)];
            if (string.IsNullOrEmpty(_product.Name))
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.NameCannotBeEmpty;
            }
            else
            {
                state.Level = ValidationLevel.Valid;
                state.Message = string.Empty;
            }

            state = _validationDictionary[nameof(Product.PriceInCents)];
            if (_product.PriceInCents < 0)
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.NegativePriceNotAllowed;
            }
            else if (_product.PriceInCents == 0)
            {
                state.Level = ValidationLevel.Warning;
                state.Message = OrderResources.SellingProductForFree;
            }
            else
            {
                state.Level = ValidationLevel.Valid;
                state.Message = string.Empty;
            }

            if (_validationDictionary.Values.Any(s => s.Level == ValidationLevel.Error))
            {
                return ValidationLevel.Error;
            }
            if(_validationDictionary.Values.Any(s => s.Level == ValidationLevel.Warning))
            {
                return ValidationLevel.Warning;
            }
            return ValidationLevel.Valid;
        }
    }
}
