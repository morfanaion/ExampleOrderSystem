using OrderSystem.Data.Classes;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using OrderSystem.Business.Orchestration.Interfaces;
using OrderSystem.Business.Resources;
using OrderSystem.Business.Services;
using OrderSystem.Business.ViewModels;
using OrderSystem.Business.ViewModels.Products;

namespace OrderSystem.Business.Orchestration
{
    public class ProductOrchestrator : IProductOrchestrator
    {
        public IDataCollection<Product> ProductsSource => DataManager.Instance.Products;

        public void AddNewProduct(ViewModel modalOwner)
        {
            Product product = DataManager.Instance.Products.CreateNew();
            IValidator validator = new ProductValidator(product);
            ProductViewModel productViewModel = new(product, validator, OrderBusinessResources.AddProduct);

            if (ServiceLocator.GetService<IUiService>().ShowDialog(productViewModel, modalOwner))
            {
                product.Save();
                ProductsSource.Add(product);
                DataManager.Instance.CommitAllChanges();
            }
            productViewModel.Dispose();
        }

        public void DeleteProduct(Product product, ViewModel modalOwner)
        {
            if(ServiceLocator.GetService<IUiService>().Confirm(OrderBusinessResources.DeleteProduct, string.Format(OrderBusinessResources.ConfirmDeleteProduct, product.Name), modalOwner))
            {
                ProductsSource.Remove(product);
                DataManager.Instance.CommitAllChanges();
            }
        }

        public void EditProduct(Product product, ViewModel modalOwner)
        {
            IValidator validator = new ProductValidator(product);
            ProductViewModel productViewModel = new(product, validator, OrderBusinessResources.EditProduct);

            if(ServiceLocator.GetService<IUiService>().ShowDialog(productViewModel, modalOwner))
            {
                product.Save();
                DataManager.Instance.CommitAllChanges();
            }
            else
            {
                product.Rollback();
            }
            productViewModel.Dispose();
        }
    }
}
