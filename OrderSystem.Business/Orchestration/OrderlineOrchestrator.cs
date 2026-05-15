using OrderSystem.Business.Orchestration.Interfaces;
using OrderSystem.Business.Resources;
using OrderSystem.Business.Services;
using OrderSystem.Business.ViewModels;
using OrderSystem.Business.ViewModels.OrderLines;
using OrderSystem.Business.ViewModels.ProductGroups;
using OrderSystem.Business.ViewModels.Products;
using OrderSystem.Data.Classes;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using OrderSystem.Data.Validation;

namespace OrderSystem.Business.Orchestration
{
    public class OrderlineOrchestrator(IDataCollection<OrderLine> source, SaveStrategy saveStrategy) : IOrderlineOrchestrator
    {
        public IDataCollection<OrderLine> OrderLinesSource => source;

        public OrderLine? AddNewOrderLine(ViewModel modalOwner)
        {
            OrderLine? orderLine = DataManager.Instance.OrderLines.CreateNew();
            IValidator validator = new OrderLineValidator(orderLine);
            OrderLineViewModel orderlineViewModel = new(orderLine, this, validator, OrderBusinessResources.AddOrderLine);

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
            else
            {
                orderLine = null;
            }
            orderlineViewModel.Dispose();
            return orderLine;
        }

        public void DeleteOrderLine(OrderLine orderLine, ViewModel modalOwner)
        {
            if (ServiceLocator.GetService<IUiService>().Confirm(OrderBusinessResources.DeleteOrderLine, string.Format(OrderBusinessResources.ConfirmDeleteOrderLine, orderLine.Product!.Name), modalOwner))
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
            OrderLineViewModel orderLineViewModel = new(orderLine, this, validator, OrderBusinessResources.EditOrderLine);
            
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

        public Product? SearchProduct(IEnumerable<Product> productsLookup, Product? selectedProduct, ViewModel modalOwner)
        {
            IProductOrchestrator productOrchestrator = new ProductOrchestrator(productsLookup);
            ProductsViewModel productsViewModel = new(productOrchestrator);
            productsViewModel.SelectedItem = selectedProduct;
            SearchViewModel<Product> searchViewModel = new(string.Format(OrderBusinessResources.SearchObject, OrderResources.Product), productsViewModel);
            if (ServiceLocator.GetService<IUiService>().ShowDialog(searchViewModel, modalOwner))
            {
                selectedProduct = productsViewModel.SelectedItem;
            }
            searchViewModel.Dispose();
            productsViewModel.Dispose();
            return selectedProduct;
        }
            
    }
}
