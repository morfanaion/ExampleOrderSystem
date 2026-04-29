using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;

namespace OrderSystem.Data.Validation
{
    public class OrderLineValidator : IValidator
    {
        public ValidationState this[string propertyName] => _validationDictionary[propertyName];
        private OrderLine _orderLine;
        private Dictionary<string, ValidationState> _validationDictionary;

        public OrderLineValidator(OrderLine orderLine)
        {
            ValidationState productState = new ValidationState() { Level = ValidationLevel.Valid };

            _validationDictionary = new Dictionary<string, ValidationState>()
            {
                { nameof(OrderLine.PricePerUnitInCents), new ValidationState() {Level = ValidationLevel.Valid} },
                { nameof(OrderLine.Product), productState },
                { nameof(OrderLine.ProductId), productState },
                { nameof(OrderLine.Quantity), new ValidationState() {Level = ValidationLevel.Valid} },
            };
            _orderLine = orderLine;
        }

        public ValidationLevel Validate()
        {
            ValidationState state = _validationDictionary[nameof(OrderLine.PricePerUnitInCents)];
            if (_orderLine.PricePerUnitInCents < 0)
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.NegativePriceNotAllowed;
            }
            else if (_orderLine.PricePerUnitInCents == 0)
            {
                state.Level = ValidationLevel.Warning;
                state.Message = OrderResources.SellingProductForFree;
            }
            else
            {
                state.Level = ValidationLevel.Valid;
                state.Message = string.Empty;
            }

            state = _validationDictionary[nameof(OrderLine.Product)];
            if (string.IsNullOrEmpty(_orderLine.Product?.Id))
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.EmptyProductNotAllowed;
            }
            else
            {
                state.Level = ValidationLevel.Valid;
                state.Message = string.Empty;
            }

            state = _validationDictionary[nameof(OrderLine.Quantity)];
            if (_orderLine.Quantity < 1)
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.QuantityCannotBeTooLow;
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
    }
}
