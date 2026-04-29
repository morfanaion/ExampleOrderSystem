using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;

namespace OrderSystem.Data.Validation
{
    public class ProductGroupValidator : IValidator
    {
        public ValidationState this[string propertyName] => _validationDictionary[propertyName];
        private ProductGroup _productGroup;
        private Dictionary<string, ValidationState> _validationDictionary;

        public ProductGroupValidator(ProductGroup productGroup)
        {
            ValidationState productgroupState = new ValidationState() { Level = ValidationLevel.Valid };

            _validationDictionary = new Dictionary<string, ValidationState>()
            {
                { nameof(ProductGroup.Id), new ValidationState() { Level = ValidationLevel.Valid } },
                { nameof(ProductGroup.Name), new ValidationState() {Level = ValidationLevel.Valid} },
            };
            _productGroup = productGroup;
        }

        public ValidationLevel Validate()
        {
            ValidationState state = _validationDictionary[nameof(ProductGroup.Id)];
            if (_productGroup.Id == string.Empty)
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.IdCannotBeEmpty;
            }
            else if (!IdUnique())
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.IdMustBeUnique;
            }
            else
            {
                state.Level = ValidationLevel.Valid;
                state.Message = string.Empty;
            }

            state = _validationDictionary[nameof(ProductGroup.Name)];
            if (_productGroup.Name == string.Empty)
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.NameCannotBeEmpty;
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
            if (_validationDictionary.Values.Any(s => s.Level == ValidationLevel.Warning))
            {
                return ValidationLevel.Warning;
            }
            return ValidationLevel.Valid;
        }

        private bool IdUnique()
        {
            IEnumerable<ProductGroup> matchedGroups = DataManager.Instance.ProductGroups.Where(pg => pg.Id == _productGroup.Id);
            if(!matchedGroups.Any())
            {
                // we are adding a new productgroup, so this is not an issue
                return true;
            }
            // there are productgroups with this id. Check if there are any that are this productgroup. If there aren't, then we are editing the only one we found
            return !matchedGroups.Any(pg => !ReferenceEquals(pg, _productGroup));
        }
    }
}
