using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using OrderSystem.WPF.Classes;
using OrderSystem.WPF.Orchestration.Interfaces;

namespace OrderSystem.WPF.ViewModels.Orders
{
    internal class OrdersViewModel : ViewModel
    {
        private readonly IOrderOrchestrator _orchestrator;

        public RelayCommand AddCommand { get; }
        public RelayCommand<Order> EditCommand { get; }
        public RelayCommand<Order> DeleteCommand { get; }

        public OrdersViewModel(IOrderOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            AddCommand = new RelayCommand(() => _orchestrator.AddNewOrder(this));
            EditCommand = new RelayCommand<Order>(order => _orchestrator.EditOrder(order!, this), order => order is not null);
            DeleteCommand = new RelayCommand<Order>(order => _orchestrator.DeleteOrder(order!, this), order => order is not null);
        }

        public IEnumerable<Order> OrdersSource => _orchestrator.OrdersSource;

        public override string ViewTitle => OrderResources.Orders;
    }
}
