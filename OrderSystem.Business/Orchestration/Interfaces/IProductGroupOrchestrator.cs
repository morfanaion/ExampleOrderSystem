using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.Business.ViewModels;

namespace OrderSystem.Business.Orchestration.Interfaces
{
    public interface IProductGroupOrchestrator
    {
        IEnumerable<ProductGroup> ProductGroupsSource { get; }
        ProductGroup? AddNewProductGroup(ViewModel modalOwner);
        void EditProductGroup(ProductGroup productGroup, ViewModel modalOwner);
        void DeleteProductGroup(ProductGroup productGroup, ViewModel modalOwner);
    }
}
