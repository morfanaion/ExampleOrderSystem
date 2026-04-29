using OrderSystem.Data.Classes;
using System.Xml.Linq;

namespace OrderSystem.Data.Models
{
    public abstract class BaseModel(XElement element) : BaseNotifier
    {
        internal XElement Element { get; } = element;

        public abstract void Save();

        public abstract void Rollback();

        public abstract bool IsDirty { get; }
    }
}
