using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.Business.ViewModels;

namespace OrderSystem.Business.Orchestration.Interfaces
{
    public interface IProductOrchestrator
    {
        IEnumerable<Product> ProductsSource { get; }
        Product? AddNewProduct(ViewModel modalOwner);
        void EditProduct(Product product, ViewModel modalOwner);
        void DeleteProduct(Product product, ViewModel modalOwner);
        ProductGroup? SearchProductGroup(IEnumerable<ProductGroup> productGroupsLookup, ProductGroup? selectedProductGroup,ViewModel modalOwner);
    }
}
