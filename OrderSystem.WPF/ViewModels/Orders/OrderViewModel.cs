using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;

namespace OrderSystem.WPF.ViewModels.Orders
{
    internal class OrderViewModel(Order order, IValidator validator, string viewTitle, ViewModel orderLinesViewModel) : ObjectViewModel<Order>(order, validator)
    {
        public override string ViewTitle { get; } = viewTitle;

        public ViewModel OrderLinesViewModel { get; } = orderLinesViewModel;
    }
}
