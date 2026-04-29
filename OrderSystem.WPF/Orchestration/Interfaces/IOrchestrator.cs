using OrderSystem.Data.Models;

namespace OrderSystem.WPF.Orchestration.Interfaces
{
    internal interface IOrchestrator<T> where T : BaseModel
    {
        void AddNewObject(T obj);
        void EditObject(T obj);
        void DeleteObject(T obj);
    }
}
