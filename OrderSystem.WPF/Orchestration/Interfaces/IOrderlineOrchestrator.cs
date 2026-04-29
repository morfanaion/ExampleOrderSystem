using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using OrderSystem.WPF.ViewModels;

namespace OrderSystem.WPF.Orchestration.Interfaces
{
    internal interface IOrderlineOrchestrator
    {
        IDataCollection<OrderLine> OrderLinesSource { get; }
        void AddNewOrderLine(ViewModel modalOwner);
        void EditOrderLine(OrderLine orderLine, ViewModel modalOwner);
        void DeleteOrderLine(OrderLine orderLine, ViewModel modalOwner);
    }
}
