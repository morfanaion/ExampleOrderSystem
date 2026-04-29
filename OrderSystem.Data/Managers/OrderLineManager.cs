using OrderSystem.Data.Models;
using System.Xml.Linq;

namespace OrderSystem.Data.Managers
{
    public class OrderLineManager : BaseManager<OrderLine>
    {
        internal OrderLineManager(XElement element) : base(element)
        {
        }

        internal const string ElementName = "OrderLines";
        internal const string NextIdAttibuteName = "NextId";

        private int GenerateId()
        {
            int nextId = GetAttribute(NextIdAttibuteName, int.Parse);
            SetAttribute(NextIdAttibuteName, (nextId + 1).ToString());
            return nextId;
        }

        public override OrderLine CreateNew() => new(GenerateId());

        private int _nextDummyId = -1;
        public override OrderLine CreateDummy() => new(_nextDummyId--);
    }
}
