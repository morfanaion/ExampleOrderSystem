using OrderSystem.Data.Managers;
using System.Xml.Linq;

namespace OrderSystem.Data.Models
{
    public class Product : BaseModel
    {
        public const string ElementName = "Product";
        public const string IdAttributeName = "Id";
        public const string ProductGroupIdAttributeName = "ProductGroupId";
        public const string NameAttributeName = "Name";
        public const string PriceInCentsAttributeName = "PriceInCents";
        public const string IsExpiredAttributeName = "IsExpired";

        internal Product(XElement element) : base(element)
        {
        }

        internal Product(string id) : this(new XElement(ElementName,
            new XAttribute(IdAttributeName, id),
            new XAttribute(ProductGroupIdAttributeName, string.Empty),
            new XAttribute(NameAttributeName, string.Empty),
            new XAttribute(PriceInCentsAttributeName, "0"),
            new XAttribute(IsExpiredAttributeName, "false")))
        {
        }

        public string Id
        {
            get => Element.Attribute(IdAttributeName)!.Value;
        }

        private string? _productGroupId = null;
        public string ProductGroupId
        {
            get => _productGroupId ?? Element.Attribute(ProductGroupIdAttributeName)!.Value;
            set
            {
                if (value == ProductGroupId)
                {
                    return;
                }
                _productGroupId = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ProductGroup));
            }
        }

        public ProductGroup? ProductGroup
        {
            get
            {
                return DataManager.Instance.ProductGroups.SingleOrDefault(pg => pg.Id == ProductGroupId);
            }
            set
            {
                if (value == null)
                {
                    ProductGroupId = string.Empty;
                }
                else
                {
                    ProductGroupId = value.Id;
                }
            }
        }

        private string? _name = null;
        public string Name
        {
            get => _name ?? Element.Attribute(NameAttributeName)!.Value;
            set
            {
                if(value == Name)
                {
                    return;
                }
                _name = value;
                RaisePropertyChanged();
            }
        }

        private int? _priceInCents = null;
        public int PriceInCents
        {
            get => _priceInCents ?? int.Parse(Element.Attribute(PriceInCentsAttributeName)!.Value);
            set
            {
                if(value == PriceInCents)
                {
                    return;
                }
                _priceInCents = value;
                RaisePropertyChanged();
            }
        }

        public bool IsExpired   
        {
            get => bool.Parse(Element.Attribute(IsExpiredAttributeName)!.Value);
            set
            {
                if(value == IsExpired)
                {
                    return;
                }
                Element.Attribute(IsExpiredAttributeName)!.Value = value.ToString();
                RaisePropertyChanged();
            }
        }

        public override bool IsDirty => _productGroupId is not null || _name is not null || _priceInCents is not null;

        public override void Save()
        {
            Element.Attribute(ProductGroupIdAttributeName)!.Value = ProductGroupId;
            Element.Attribute(NameAttributeName)!.Value = Name;
            Element.Attribute(PriceInCentsAttributeName)!.Value = PriceInCents.ToString();
            Rollback();
        }

        public override void Rollback()
        {
            _productGroupId = null;
            _name = null;
            _priceInCents = null;
            RaisePropertyChanged(nameof(ProductGroupId));
            RaisePropertyChanged(nameof(ProductGroup));
            RaisePropertyChanged(nameof(Name));
            RaisePropertyChanged(nameof(PriceInCents));
        }
    }
}
