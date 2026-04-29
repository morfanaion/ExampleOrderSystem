using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.WPF.ViewModels;

namespace OrderSystem.WPF.Orchestration.Interfaces
{
    internal interface IProductGroupOrchestrator
    {
        IDataCollection<ProductGroup> ProductGroupsSource { get; }
        void AddNewProductGroup(ViewModel modalOwner);
        void EditProductGroup(ProductGroup productGroup, ViewModel modalOwner);
        void DeleteProductGroup(ProductGroup productGroup, ViewModel modalOwner);
    }
}
