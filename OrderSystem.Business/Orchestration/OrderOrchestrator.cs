using OrderSystem.Data.Classes;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using OrderSystem.Business.Orchestration.Interfaces;
using OrderSystem.Business.Resources;
using OrderSystem.Business.Services;
using OrderSystem.Business.ViewModels;
using OrderSystem.Business.ViewModels.OrderLines;
using OrderSystem.Business.ViewModels.Orders;

namespace OrderSystem.Business.Orchestration
{
    public class OrderOrchestrator : IOrderOrchestrator
    {
        public IDataCollection<Order> OrdersSource => DataManager.Instance.Orders;

        public Order? AddNewOrder(ViewModel modalOwner)
        {
            Order? order = DataManager.Instance.Orders.CreateNew();
            IValidator validator = new OrderValidator(order);
            IOrderlineOrchestrator orderLinesOrchestrator = new OrderlineOrchestrator(order.OrderLines, SaveStrategy.DeferredSave);
            ViewModel orderLinesViewmodel = new OrderLinesViewModel(orderLinesOrchestrator);
            OrderViewModel productViewModel = new(order, validator, OrderBusinessResources.AddOrder, orderLinesViewmodel);

            if (ServiceLocator.GetService<IUiService>().ShowDialog(productViewModel, modalOwner))
            {
                order.Save();
                OrdersSource.Add(order);
                DataManager.Instance.CommitAllChanges();
            }
            else
            {
                order = null;
            }
            productViewModel.Dispose();
            return order;
        }

        public void DeleteOrder(Order order, ViewModel modalOwner)
        {
            if (ServiceLocator.GetService<IUiService>().Confirm(OrderBusinessResources.DeleteOrder, string.Format(OrderBusinessResources.ConfirmDeleteOrder, order.PlacedAt.ToShortDateString()), modalOwner))
            {
                OrdersSource.Remove(order);
                DataManager.Instance.CommitAllChanges();
            }
        }

        public void EditOrder(Order order, ViewModel modalOwner)
        {
            IValidator validator = new OrderValidator(order);
            IOrderlineOrchestrator orderLinesOrchestrator = new OrderlineOrchestrator(order.OrderLines, SaveStrategy.DeferredSave);
            ViewModel orderLinesViewmodel = new OrderLinesViewModel(orderLinesOrchestrator);
            OrderViewModel productViewModel = new(order, validator, OrderBusinessResources.EditOrder, orderLinesViewmodel);

            if (ServiceLocator.GetService<IUiService>().ShowDialog(productViewModel, modalOwner))
            {
                order.Save();
                DataManager.Instance.CommitAllChanges();
            }
            else
            {
                order.Rollback();
            }
            productViewModel.Dispose();
        }

        public Product? SearchProduct(IEnumerable<Product> productFilterLookup, Product? selectedFilterProduct, OrdersViewModel ordersViewModel)
        {
            IOrderlineOrchestrator orderlineOrchestrator = new OrderlineOrchestrator(DataManager.Instance.OrderLines, SaveStrategy.DeferredSave);
            return orderlineOrchestrator.SearchProduct(productFilterLookup, selectedFilterProduct, ordersViewModel);
        }
    }
}
