using OrderSystem.Data.Managers;
using System.Xml.Linq;

namespace OrderSystem.Data.Models
{
    public class ProductGroup : BaseModel
    {
        public const string ElementName = "ProductGroup";
        public const string IdAttributeName = "Id";
        public const string NameAttributeName = "Name";
        public const string IsExpiredAttributeName = "IsExpired";

        internal ProductGroup(XElement element) : base(element)
        {
        }

        internal ProductGroup() : this(new XElement(ElementName,
            new XAttribute(IdAttributeName, string.Empty),
            new XAttribute(NameAttributeName, string.Empty),
            new XAttribute(IsExpiredAttributeName, "false")))
        {
        }

        private string? _id = null;
        public string Id
        {
            get => _id ?? Element.Attribute(IdAttributeName)!.Value;
            set
            {
                if (value == Id)
                {
                    return;
                }
                _id = value;
                RaisePropertyChanged();
            }
        }

        private string? _name = null;
        public string Name
        {
            get => _name ?? Element.Attribute(NameAttributeName)!.Value;
            set
            {
                if (value == Name)
                {
                    return;
                }
                _name = value;
                RaisePropertyChanged();
            }
        }

        public bool IsExpired
        {
            get => bool.Parse(Element.Attribute(IsExpiredAttributeName)!.Value);
            set
            {
                if (value == IsExpired)
                {
                    return;
                }
                Element.Attribute(IsExpiredAttributeName)!.Value = value.ToString();
                RaisePropertyChanged();
            }
        }

        public override bool IsDirty => _name is not null || _id is not null;

        public override void Save()
        {
            string oldId = Element.Attribute(IdAttributeName)!.Value;
            Element.Attribute(IdAttributeName)!.Value = Id;
            Element.Attribute(NameAttributeName)!.Value = Name;
            Rollback();
            if (oldId == Id)
            {
                return;
            }
            foreach (Product product in DataManager.Instance.Products.Where(p => p.ProductGroupId == oldId))
            {
                product.ProductGroupId = Id;
                product.Save();
            }
        }

        public override void Rollback()
        {
            _id = null;
            _name = null;
            RaisePropertyChanged(nameof(Id));
            RaisePropertyChanged(nameof(Name));
        }
    }
}

