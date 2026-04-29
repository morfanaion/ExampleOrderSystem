using OrderSystem.Data.Classes;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using OrderSystem.WPF.Orchestration.Interfaces;
using OrderSystem.WPF.Resources;
using OrderSystem.WPF.Services;
using OrderSystem.WPF.ViewModels;
using OrderSystem.WPF.ViewModels.Products;

namespace OrderSystem.WPF.Orchestration
{
    internal class ProductOrchestrator : IProductOrchestrator
    {
        public IDataCollection<Product> ProductsSource => DataManager.Instance.Products;

        public void AddNewProduct(ViewModel modalOwner)
        {
            Product product = DataManager.Instance.Products.CreateNew();
            IValidator validator = new ProductValidator(product);
            ProductViewModel productViewModel = new(product, validator, WpfOrderResources.AddProduct);

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
            if(ServiceLocator.GetService<IUiService>().Confirm(WpfOrderResources.DeleteProduct, string.Format(WpfOrderResources.ConfirmDeleteProduct, product.Name), modalOwner))
            {
                ProductsSource.Remove(product);
                DataManager.Instance.CommitAllChanges();
            }
        }

        public void EditProduct(Product product, ViewModel modalOwner)
        {
            IValidator validator = new ProductValidator(product);
            ProductViewModel productViewModel = new(product, validator, WpfOrderResources.EditProduct);

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
