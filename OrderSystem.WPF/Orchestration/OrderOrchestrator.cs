using OrderSystem.Data.Classes;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using OrderSystem.WPF.Orchestration.Interfaces;
using OrderSystem.WPF.Resources;
using OrderSystem.WPF.Services;
using OrderSystem.WPF.ViewModels;
using OrderSystem.WPF.ViewModels.OrderLines;
using OrderSystem.WPF.ViewModels.Orders;

namespace OrderSystem.WPF.Orchestration
{
    internal class OrderOrchestrator : IOrderOrchestrator
    {
        public IDataCollection<Order> OrdersSource => DataManager.Instance.Orders;

        public void AddNewOrder(ViewModel modalOwner)
        {
            Order order = DataManager.Instance.Orders.CreateNew();
            IValidator validator = new OrderValidator(order);
            IOrderlineOrchestrator orderLinesOrchestrator = new OrderlineOrchestrator(order.OrderLines, SaveStrategy.DeferredSave);
            ViewModel orderLinesViewmodel = new OrderLinesViewModel(orderLinesOrchestrator);
            OrderViewModel productViewModel = new(order, validator, WpfOrderResources.AddOrder, orderLinesViewmodel);

            if (ServiceLocator.GetService<IUiService>().ShowDialog(productViewModel, modalOwner))
            {
                order.Save();
                OrdersSource.Add(order);
                DataManager.Instance.CommitAllChanges();
            }
            productViewModel.Dispose();
        }

        public void DeleteOrder(Order order, ViewModel modalOwner)
        {
            if (ServiceLocator.GetService<IUiService>().Confirm(WpfOrderResources.DeleteOrder, string.Format(WpfOrderResources.ConfirmDeleteOrder, order.PlacedAt.ToShortDateString()), modalOwner))
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
            OrderViewModel productViewModel = new(order, validator, WpfOrderResources.EditOrder, orderLinesViewmodel);

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
    }
}
