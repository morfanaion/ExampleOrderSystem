using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using OrderSystem.Business.Classes;
using OrderSystem.Business.Orchestration.Interfaces;

namespace OrderSystem.Business.ViewModels.OrderLines
{
    public class OrderLinesViewModel : ListViewModel<OrderLine>
    {
        private readonly IOrderlineOrchestrator _orchestrator;

        public OrderLinesViewModel(IOrderlineOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            AddCommand = new RelayCommand(() =>
            {
                if(_orchestrator.AddNewOrderLine(this) is OrderLine newOrderLine)
                {
                    SelectedItem = newOrderLine;
                }
            });
            EditCommand = new RelayCommand<OrderLine>(orderLine => _orchestrator.EditOrderLine(orderLine!, this), orderLine => orderLine is not null);
            DeleteCommand = new RelayCommand<OrderLine>(orderLine => _orchestrator.DeleteOrderLine(orderLine!, this), orderLine => orderLine is not null);
        }

        public IEnumerable<OrderLine> OrderLinesSource => _orchestrator.OrderLinesSource;

        public override string ViewTitle => OrderResources.OrderLines;
    }
}
