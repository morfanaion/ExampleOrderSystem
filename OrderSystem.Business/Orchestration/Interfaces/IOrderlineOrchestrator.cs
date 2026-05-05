using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.Business.ViewModels;

namespace OrderSystem.Business.Orchestration.Interfaces
{
    public interface IOrderlineOrchestrator
    {
        IDataCollection<OrderLine> OrderLinesSource { get; }
        void AddNewOrderLine(ViewModel modalOwner);
        void EditOrderLine(OrderLine orderLine, ViewModel modalOwner);
        void DeleteOrderLine(OrderLine orderLine, ViewModel modalOwner);
    }
}
