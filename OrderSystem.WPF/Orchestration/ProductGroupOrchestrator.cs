using OrderSystem.Data.Classes;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using OrderSystem.WPF.Orchestration.Interfaces;
using OrderSystem.WPF.Resources;
using OrderSystem.WPF.Services;
using OrderSystem.WPF.ViewModels;
using OrderSystem.WPF.ViewModels.ProductGroups;

namespace OrderSystem.WPF.Orchestration
{
    internal class ProductGroupOrchestrator : IProductGroupOrchestrator
    {
        public IDataCollection<ProductGroup> ProductGroupsSource => DataManager.Instance.ProductGroups;

        public void AddNewProductGroup(ViewModel modalOwner)
        {
            ProductGroup group = DataManager.Instance.ProductGroups.CreateNew();
            IValidator validator = new ProductGroupValidator(group);
            ProductGroupViewModel viewModel = new(group, validator, WpfOrderResources.AddProductGroup);

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
            if (ServiceLocator.GetService<IUiService>().Confirm(WpfOrderResources.DeleteProductGroup, string.Format(WpfOrderResources.ConfirmDeleteProductGroup, productGroup.Name), modalOwner))
            {
                ProductGroupsSource.Remove(productGroup);
                DataManager.Instance.CommitAllChanges();
            }
        }

        public void EditProductGroup(ProductGroup productGroup, ViewModel modalOwner)
        {
            IValidator validator = new ProductGroupValidator(productGroup);
            ProductGroupViewModel viewModel = new(productGroup, validator, WpfOrderResources.EditProductGroup);

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
