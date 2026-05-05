using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.Business.ViewModels;

namespace OrderSystem.Business.Orchestration.Interfaces
{
    public interface IOrderOrchestrator
    {
        IDataCollection<Order> OrdersSource { get; }
        void AddNewOrder(ViewModel modalOwner);
        void EditOrder(Order order, ViewModel modalOwner);
        void DeleteOrder(Order order, ViewModel modalOwner);
    }
}
