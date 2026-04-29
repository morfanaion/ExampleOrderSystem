using OrderSystem.Data.Models;
using System.Xml.Linq;

namespace OrderSystem.Data.Managers
{
    public class OrderManager : BaseManager<Order>
    {
        internal OrderManager(XElement element) : base(element)
        {

        }

        internal const string ElementName = "Orders";
        internal const string NextIdAttibuteName = "NextId";

        private int GenerateId()
        {
            int nextId = GetAttribute(NextIdAttibuteName, int.Parse);
            SetAttribute(NextIdAttibuteName, (nextId + 1).ToString());
            return nextId;
        }

        public override Order CreateNew() => new(GenerateId());

        private int _nextDummyId = -1;
        public override Order CreateDummy() => new(_nextDummyId--);

        public override void Remove(Order item)
        {
            foreach(OrderLine orderLine in item.OrderLines)
            {
                DataManager.Instance.OrderLines.Remove(orderLine);
            }
            base.Remove(item);
        }
    }
}
