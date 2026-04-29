using OrderSystem.Data.Classes;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using OrderSystem.WPF.Orchestration.Interfaces;
using OrderSystem.WPF.Resources;
using OrderSystem.WPF.Services;
using OrderSystem.WPF.ViewModels;
using OrderSystem.WPF.ViewModels.OrderLines;

namespace OrderSystem.WPF.Orchestration
{
    internal class OrderlineOrchestrator(IDataCollection<OrderLine> source, SaveStrategy saveStrategy) : IOrderlineOrchestrator
    {
        public IDataCollection<OrderLine> OrderLinesSource => source;

        public void AddNewOrderLine(ViewModel modalOwner)
        {
            OrderLine orderLine = DataManager.Instance.OrderLines.CreateNew();
            IValidator validator = new OrderLineValidator(orderLine);
            OrderLineViewModel orderlineViewModel = new(orderLine, validator, WpfOrderResources.AddOrderLine);

            if (ServiceLocator.GetService<IUiService>().ShowDialog(orderlineViewModel, modalOwner))
            {
                OrderLinesSource.Add(orderLine);
                if (saveStrategy == SaveStrategy.ImmediateSave)
                {
                    orderLine.Save();
                    DataManager.Instance.CommitAllChanges();
                }
                else if(saveStrategy == SaveStrategy.DeferredSave)
                {
                    orderLine.SaveCached();
                }
            }
            orderlineViewModel.Dispose();
        }

        public void DeleteOrderLine(OrderLine orderLine, ViewModel modalOwner)
        {
            if (ServiceLocator.GetService<IUiService>().Confirm(WpfOrderResources.DeleteOrderLine, string.Format(WpfOrderResources.ConfirmDeleteOrderLine, orderLine.Product!.Name), modalOwner))
            {
                OrderLinesSource.Remove(orderLine);
                if (saveStrategy == SaveStrategy.ImmediateSave)
                {
                    DataManager.Instance.CommitAllChanges();
                }
            }
        }

        public void EditOrderLine(OrderLine orderLine, ViewModel modalOwner)
        {
            IValidator validator = new OrderLineValidator(orderLine);
            OrderLineViewModel orderLineViewModel = new(orderLine, validator, WpfOrderResources.EditOrderLine);
            
            if (ServiceLocator.GetService<IUiService>().ShowDialog(orderLineViewModel, modalOwner))
            {
                if (saveStrategy == SaveStrategy.ImmediateSave)
                {
                    orderLine.Save();
                    DataManager.Instance.CommitAllChanges();
                }
                else if (saveStrategy == SaveStrategy.DeferredSave)
                {
                    orderLine.SaveCached();
                }
            }
            else
            {
                orderLine.Rollback();
            }
            orderLineViewModel.Dispose();
        }
    }
}
