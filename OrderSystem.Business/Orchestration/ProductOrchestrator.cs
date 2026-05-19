using OrderSystem.Data.Classes;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using OrderSystem.Business.Orchestration.Interfaces;
using OrderSystem.Business.Resources;
using OrderSystem.Business.Services;
using OrderSystem.Business.ViewModels;
using OrderSystem.Business.ViewModels.Products;
using OrderSystem.Business.ViewModels.ProductGroups;
using OrderSystem.Data.Resources;

namespace OrderSystem.Business.Orchestration
{
    public class ProductOrchestrator(IEnumerable<Product> source) : IProductOrchestrator
    {
        public IEnumerable<Product> ProductsSource => source;

        public Product? AddNewProduct(ViewModel modalOwner)
        {
            Product? product = DataManager.Instance.Products.CreateNew();
            IValidator validator = new ProductValidator(product);
            ProductViewModel productViewModel = new(product, this, validator, OrderBusinessResources.AddProduct);

            if (ServiceLocator.GetService<IUiService>().ShowDialog(productViewModel, modalOwner))
            {
                product.Save();
                DataManager.Instance.Products.Add(product);
                DataManager.Instance.CommitAllChanges();
            }
            else
            {
                product = null;
            }
            productViewModel.Dispose();
            return product;
        }

        public void DeleteProduct(Product product, ViewModel modalOwner)
        {
            if(ServiceLocator.GetService<IUiService>().Confirm(OrderBusinessResources.DeleteProduct, string.Format(OrderBusinessResources.ConfirmDeleteProduct, product.Name), modalOwner))
            {
                DataManager.Instance.Products.Remove(product);
                DataManager.Instance.CommitAllChanges();
            }
        }

        public void EditProduct(Product product, ViewModel modalOwner)
        {
            IValidator validator = new ProductValidator(product);
            ProductViewModel productViewModel = new(product, this, validator, OrderBusinessResources.EditProduct);

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

        public ProductGroup? SearchProductGroup(IEnumerable<ProductGroup> productGroupsLookup, ProductGroup? selectedProductGroup, ViewModel modalOwner)
        {
            IProductGroupOrchestrator productGroupOrchestrator = new ProductGroupOrchestrator(productGroupsLookup);
            ProductGroupsViewModel productGroupsViewModel = new(productGroupOrchestrator);
            productGroupsViewModel.SelectedItem = selectedProductGroup;
            SearchViewModel<ProductGroup> searchViewModel = new(string.Format(OrderBusinessResources.SearchObject, OrderResources.ProductGroup), productGroupsViewModel);
            if (ServiceLocator.GetService<IUiService>().ShowDialog(searchViewModel, modalOwner))
            {
                selectedProductGroup = productGroupsViewModel.SelectedItem;
            }
            searchViewModel.Dispose();
            productGroupsViewModel.Dispose();
            return selectedProductGroup;
        }
    }
}
