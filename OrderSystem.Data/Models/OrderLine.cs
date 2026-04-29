using OrderSystem.Data.Managers;
using System.Xml.Linq;

namespace OrderSystem.Data.Models
{
    public class OrderLine : BaseModel
    {
        public const string ElementName = "OrderLine";
        public const string IdAttributeName = "Id";
        public const string OrderIdAttributeName = "OrderId";
        public const string ProductIdAttributeName = "ProductId";
        public const string PricePerUnitInCentsAttributeName = "PricePerUnitInCents";
        public const string QuantityAttributeName = "Quantity";

        internal OrderLine(XElement element) : base(element) 
        { 
        }

        internal OrderLine(int id) : this(new XElement(ElementName,
            new XAttribute(IdAttributeName, id.ToString()),
            new XAttribute(OrderIdAttributeName, "0"),
            new XAttribute(ProductIdAttributeName, string.Empty),
            new XAttribute(PricePerUnitInCentsAttributeName, "0"),
            new XAttribute(QuantityAttributeName, "1")))
        {
        }

        public int Id
        {
            get => int.Parse(Element.Attribute(IdAttributeName)!.Value);
        }

        private int? _orderId = null;
        public int OrderId
        {
            get => _orderId ?? int.Parse(Element.Attribute(OrderIdAttributeName)!.Value);
            set
            {
                if(value == OrderId)
                {
                    return;
                }
                _orderId = value;
                RaisePropertyChanged();
            }
        }

        private string? _productId = null;
        private string? _cachedProductId = null;
        public string ProductId
        {
            get => _productId ?? _cachedProductId ?? Element.Attribute(ProductIdAttributeName)!.Value;
            set
            {
                if(value == ProductId)
                {
                    return;
                }

                _productId = value;
                PricePerUnitInCents = Product?.PriceInCents ?? 0;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Product));
            }
        }

        public Product? Product
        {
            get => DataManager.Instance.Products.SingleOrDefault(p => p.Id == ProductId);
            set
            {
                if (value == null)
                {
                    ProductId = string.Empty;
                }
                else
                {
                    ProductId = value.Id;
                }
            }
        }

        private int? _pricePerUnitInCents = null;
        private int? _cachedPricePerUnitInCents = null;
        public int PricePerUnitInCents
        {
            get => _pricePerUnitInCents ?? _cachedPricePerUnitInCents ?? int.Parse(Element.Attribute(PricePerUnitInCentsAttributeName)!.Value);
            set
            {
                if(value == PricePerUnitInCents)
                {
                    return;
                }
                _pricePerUnitInCents = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TotalPriceInCents));
            }
        }

        private int? _quantity = null;
        private int? _cachedQuantity = null;
        public int Quantity
        {
            get => _quantity ?? _cachedQuantity ?? int.Parse(Element.Attribute(QuantityAttributeName)!.Value);
            set
            {
                if(value == Quantity)
                {
                    return;
                }
                _quantity = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TotalPriceInCents));
            }
        }

        public int TotalPriceInCents => PricePerUnitInCents * Quantity;

        public override bool IsDirty => _orderId is not null || _quantity is not null || _pricePerUnitInCents is not null || _productId is not null;

        public override void Save()
        {
            Element.Attribute(OrderIdAttributeName)!.Value = OrderId.ToString();
            Element.Attribute(ProductIdAttributeName)!.Value = ProductId;
            Element.Attribute(PricePerUnitInCentsAttributeName)!.Value = PricePerUnitInCents.ToString();
            Element.Attribute(QuantityAttributeName)!.Value = Quantity.ToString();
            Rollback();
        }

        public void SaveCached()
        {
            _cachedProductId = ProductId;
            _cachedPricePerUnitInCents = PricePerUnitInCents;
            _cachedQuantity = Quantity;
            Rollback();
        }

        public void ClearCacheAndRollback()
        {
            _cachedProductId = null;
            _cachedPricePerUnitInCents = null;
            _cachedQuantity = null;
        }

        public override void Rollback()
        {
            _orderId = null;
            _productId = null;
            _pricePerUnitInCents = null;
            _quantity = null;
            RaisePropertyChanged(nameof(OrderId));
            RaisePropertyChanged(nameof(ProductId));
            RaisePropertyChanged(nameof(Product));
            RaisePropertyChanged(nameof(PricePerUnitInCents));
            RaisePropertyChanged(nameof(Quantity));
            RaisePropertyChanged(nameof(TotalPriceInCents));
        }
    }
}
