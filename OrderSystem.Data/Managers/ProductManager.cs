using OrderSystem.Data.Models;
using System.Xml.Linq;

namespace OrderSystem.Data.Managers
{
    public class ProductManager : BaseManager<Product>
    {
        internal ProductManager(XElement element) : base(element)
        {
        }

        internal const string ElementName = "Products";
        internal const string NextIdAttibuteName = "NextId";

        private string GenerateId()
        {
            string nextId = GetAttribute(NextIdAttibuteName);
            SetAttribute(NextIdAttibuteName, (int.Parse(nextId) + 1).ToString("0000"));
            return nextId;
        }

        public override Product CreateNew()
        {
            return new Product(GenerateId());
        }

        private int _nextDummyId = -1;
        public override Product CreateDummy() => new((_nextDummyId--).ToString());

        public override void Remove(Product item)
        {
            item.IsExpired = true;
            RaiseReplace(item, item);
        }
    }
}
