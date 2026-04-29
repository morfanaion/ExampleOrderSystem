using OrderSystem.Data.Models;
using System.Xml.Linq;

namespace OrderSystem.Data.Managers
{
    public class ProductGroupManager : BaseManager<ProductGroup>
    {
        internal ProductGroupManager(XElement element) : base(element)
        {
        }

        internal const string ElementName = "ProductGroups";
        
        public override ProductGroup CreateNew() => new();

        public override ProductGroup CreateDummy() => new();

        public override void Remove(ProductGroup item)
        {
            item.IsExpired = true;
            RaiseReplace(item, item);
        }
    }
}
