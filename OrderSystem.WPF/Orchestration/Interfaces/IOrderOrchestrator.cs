using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.WPF.ViewModels;

namespace OrderSystem.WPF.Orchestration.Interfaces
{
    internal interface IOrderOrchestrator
    {
        IDataCollection<Order> OrdersSource { get; }
        void AddNewOrder(ViewModel modalOwner);
        void EditOrder(Order order, ViewModel modalOwner);
        void DeleteOrder(Order order, ViewModel modalOwner);
    }
}
