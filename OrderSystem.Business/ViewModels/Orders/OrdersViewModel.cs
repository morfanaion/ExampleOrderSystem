using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using OrderSystem.Business.Classes;
using OrderSystem.Business.Orchestration.Interfaces;
using System.Windows.Input;

namespace OrderSystem.Business.ViewModels.Orders
{
    public class OrdersViewModel : ViewModel
    {
        private readonly IOrderOrchestrator _orchestrator;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

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
