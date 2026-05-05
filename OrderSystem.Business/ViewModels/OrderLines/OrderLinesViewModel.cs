using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using OrderSystem.Business.Classes;
using OrderSystem.Business.Orchestration.Interfaces;
using System.Windows.Input;

namespace OrderSystem.Business.ViewModels.OrderLines
{
    public class OrderLinesViewModel : ViewModel
    {
        private readonly IOrderlineOrchestrator _orchestrator;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

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
