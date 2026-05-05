using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.Business.ViewModels;

namespace OrderSystem.Business.Orchestration.Interfaces
{
    public interface IProductGroupOrchestrator
    {
        IDataCollection<ProductGroup> ProductGroupsSource { get; }
        void AddNewProductGroup(ViewModel modalOwner);
        void EditProductGroup(ProductGroup productGroup, ViewModel modalOwner);
        void DeleteProductGroup(ProductGroup productGroup, ViewModel modalOwner);
    }
}
