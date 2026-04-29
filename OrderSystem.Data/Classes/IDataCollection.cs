using OrderSystem.Data.Models;

namespace OrderSystem.Data.Classes
{
    public interface IDataCollection<T> : IEnumerable<T> where T: BaseModel
    {
        void Add(T newItem);
        void Remove(T oldItem);
    }
}
