using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.Business.ViewModels;
using OrderSystem.Business.ViewModels.Orders;

namespace OrderSystem.Business.Orchestration.Interfaces
{
    public interface IOrderOrchestrator
    {
        IDataCollection<Order> OrdersSource { get; }
        Order? AddNewOrder(ViewModel modalOwner);
        void EditOrder(Order order, ViewModel modalOwner);
        void DeleteOrder(Order order, ViewModel modalOwner);
        Product? SearchProduct(IEnumerable<Product> productFilterLookup, Product? selectedFilterProduct, OrdersViewModel ordersViewModel);
    }
}
