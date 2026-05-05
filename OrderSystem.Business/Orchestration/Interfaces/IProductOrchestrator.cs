using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.Business.ViewModels;

namespace OrderSystem.Business.Orchestration.Interfaces
{
    public interface IProductOrchestrator
    {
        IDataCollection<Product> ProductsSource { get; }
        void AddNewProduct(ViewModel modalOwner);
        void EditProduct(Product product, ViewModel modalOwner);
        void DeleteProduct(Product product, ViewModel modalOwner);
    }
}
