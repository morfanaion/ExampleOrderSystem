using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using OrderSystem.WPF.Classes;
using OrderSystem.WPF.Orchestration.Interfaces;

namespace OrderSystem.WPF.ViewModels.OrderLines
{
    internal class OrderLinesViewModel : ViewModel
    {
        private readonly IOrderlineOrchestrator _orchestrator;

        public RelayCommand AddCommand { get; }
        public RelayCommand<OrderLine> EditCommand { get; }
        public RelayCommand<OrderLine> DeleteCommand { get; }

        public OrderLinesViewModel(IOrderlineOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            AddCommand = new RelayCommand(() => _orchestrator.AddNewOrderLine(this));
            EditCommand = new RelayCommand<OrderLine>(orderLine => _orchestrator.EditOrderLine(orderLine!, this), orderLine => orderLine is not null);
            DeleteCommand = new RelayCommand<OrderLine>(orderLine => _orchestrator.DeleteOrderLine(orderLine!, this), orderLine => orderLine is not null);
        }

        public IEnumerable<OrderLine> OrderLinesSource => _orchestrator.OrderLinesSource;

        public override string ViewTitle => OrderResources.OrderLines;
    }
}
