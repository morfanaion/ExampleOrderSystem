using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;

namespace OrderSystem.Data.Validation
{
    public class OrderValidator : IValidator
    {
        public ValidationState this[string propertyName] => _validationDictionary[propertyName];
        private readonly Order _order;
        private readonly Dictionary<string, ValidationState> _validationDictionary;

        public OrderValidator(Order order)
        {
            ValidationState productState = new() { Level = ValidationLevel.Valid };

            _validationDictionary = new Dictionary<string, ValidationState>()
            {
                { nameof(Order.PlacedAt), new ValidationState() {Level = ValidationLevel.Valid} },
                { nameof(Order.OrderLines), productState }
            };
            _order = order;
        }

        public ValidationLevel Validate()
        {
            ValidationState state = _validationDictionary[nameof(Order.PlacedAt)];
            if (_order.PlacedAt > DateTime.Now)
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.OrderCannotBePlaceInFuture;
            }
            else
            {
                state.Level = ValidationLevel.Valid;
                state.Message = string.Empty;
            }

            state = _validationDictionary[nameof(Order.OrderLines)];
            if (!_order.OrderLines.Any())
            {
                state.Level = ValidationLevel.Error;
                state.Message = OrderResources.NeedToOrderAtLeast1Item;
            }
            else if(_order.OrderLines.GroupBy(o => o.ProductId).Any(g => g.Skip(1).Any()))
            {
                state.Level = ValidationLevel.Warning;
                state.Message = OrderResources.MoreThanOneLineForSameProduct;
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
