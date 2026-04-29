using OrderSystem.Data.Models;
using System.Xml.Linq;

namespace OrderSystem.Data.Managers
{
    public class DataManager
    {
        private const string OrderLinesElementName = "OrderLines";
        private const string ProductGroupsElementName = "ProductGroups";

        private static DataManager? _instance = null;
        public static DataManager Instance => _instance ??= new DataManager();

        public OrderManager Orders { get; private set; } = null!;
        public OrderLineManager OrderLines { get; private set; } = null!;
        public ProductManager Products { get; private set; } = null!;
        public ProductGroupManager ProductGroups { get; private set; } = null!;

        private XDocument _theDocument = null!;
        
        public DataManager()
        {
            RevertAllChanges();
        }

        public void RevertAllChanges()
        {
            if (!File.Exists("OrderSystemData.xml"))
            {
                _theDocument = XDocument.Load("ProductCatalog.xml");
                _theDocument.Root!.Add(new XElement(OrderManager.ElementName, new XAttribute(OrderManager.NextIdAttibuteName, "1")));
                _theDocument.Root!.Add(new XElement(OrderLineManager.ElementName, new XAttribute(OrderLineManager.NextIdAttibuteName, "1")));
                _theDocument.Root!.Element(ProductManager.ElementName)!.Add(new XAttribute(ProductManager.NextIdAttibuteName, "0035"));
                foreach(XElement productElement in _theDocument.Root!.Element(ProductManager.ElementName)!.Elements())
                {
                    productElement.Add(new XAttribute(Product.IsExpiredAttributeName, "false"));
                }
                foreach (XElement productGroupElement in _theDocument.Root!.Element(ProductGroupManager.ElementName)!.Elements())
                {
                    productGroupElement.Add(new XAttribute(ProductGroup.IsExpiredAttributeName, "false"));
                }
            }
            else
            {
                _theDocument = XDocument.Load("OrderSystemData.xml");
            }
            Orders = new OrderManager(_theDocument.Root!.Element(OrderManager.ElementName)!);
            OrderLines = new OrderLineManager(_theDocument.Root!.Element(OrderLineManager.ElementName)!);
            Products = new ProductManager(_theDocument.Root!.Element(ProductManager.ElementName)!);
            ProductGroups = new ProductGroupManager(_theDocument.Root!.Element(ProductGroupManager.ElementName)!);
        }

        public void CommitAllChanges()
        {
            _theDocument.Save("OrderSystemData.xml");
        }
    }
}
