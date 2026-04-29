using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.WPF.ViewModels;

namespace OrderSystem.WPF.Orchestration.Interfaces
{
    internal interface IProductOrchestrator
    {
        IDataCollection<Product> ProductsSource { get; }
        void AddNewProduct(ViewModel modalOwner);
        void EditProduct(Product product, ViewModel modalOwner);
        void DeleteProduct(Product product, ViewModel modalOwner);
    }
}
