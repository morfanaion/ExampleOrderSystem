using OrderSystem.Data.Classes;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using OrderSystem.Business.Orchestration.Interfaces;
using OrderSystem.Business.Resources;
using OrderSystem.Business.Services;
using OrderSystem.Business.ViewModels;
using OrderSystem.Business.ViewModels.ProductGroups;

namespace OrderSystem.Business.Orchestration
{
    public class ProductGroupOrchestrator : IProductGroupOrchestrator
    {
        public IDataCollection<ProductGroup> ProductGroupsSource => DataManager.Instance.ProductGroups;

        public void AddNewProductGroup(ViewModel modalOwner)
        {
            ProductGroup group = DataManager.Instance.ProductGroups.CreateNew();
            IValidator validator = new ProductGroupValidator(group);
            ProductGroupViewModel viewModel = new(group, validator, OrderBusinessResources.AddProductGroup);

            if(ServiceLocator.GetService<IUiService>().ShowDialog(viewModel, modalOwner))
            {
                group.Save();
                ProductGroupsSource.Add(group);
                DataManager.Instance.CommitAllChanges();
            }
            viewModel.Dispose();
        }

        public void DeleteProductGroup(ProductGroup productGroup, ViewModel modalOwner)
        {
            if (ServiceLocator.GetService<IUiService>().Confirm(OrderBusinessResources.DeleteProductGroup, string.Format(OrderBusinessResources.ConfirmDeleteProductGroup, productGroup.Name), modalOwner))
            {
                ProductGroupsSource.Remove(productGroup);
                DataManager.Instance.CommitAllChanges();
            }
        }

        public void EditProductGroup(ProductGroup productGroup, ViewModel modalOwner)
        {
            IValidator validator = new ProductGroupValidator(productGroup);
            ProductGroupViewModel viewModel = new(productGroup, validator, OrderBusinessResources.EditProductGroup);

            if (ServiceLocator.GetService<IUiService>().ShowDialog(viewModel, modalOwner))
            {
                productGroup.Save();
                DataManager.Instance.CommitAllChanges();
            }
            else
            {
                productGroup.Rollback();
            }
            viewModel.Dispose();
        }
    }
}
