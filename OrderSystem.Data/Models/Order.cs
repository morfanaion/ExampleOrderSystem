using OrderSystem.Data.Classes;
using OrderSystem.Data.Managers;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Linq;

namespace OrderSystem.Data.Models
{
    public class Order : BaseModel
    {
        public const string ElementName = "Order";
        public const string IdAttributeName = "Id";
        public const string PlacedAtAttributeName = "PlacedAt";
        
        internal Order(XElement element) : base(element)
        { 
        }

        internal Order(int id) : this(new XElement(ElementName,
            new XAttribute(IdAttributeName, id.ToString()),
            new XAttribute(PlacedAtAttributeName, DateTime.Today.ToString())))
        {
        }

        public int Id
        {
            get => int.Parse(Element.Attribute(IdAttributeName)!.Value);
        }

        private DateTime? _placedAt = null;
        public DateTime PlacedAt
        {
            get => _placedAt ?? DateTime.Parse(Element.Attribute(PlacedAtAttributeName)!.Value);
            set
            {
                if(value == PlacedAt)
                {
                    return;
                }
                _placedAt = value;
                RaisePropertyChanged();
            }
        }

        public int TotalPriceInCents => OrderLines.Sum(l => l.TotalPriceInCents);

        private RelationalList<OrderLine>? _orderLines = null;
        public RelationalList<OrderLine> OrderLines
        {
            get
            {
                if (_orderLines == null)
                {
                    _orderLines = new RelationalList<OrderLine>(DataManager.Instance.OrderLines.Where(line => line.OrderId == Id), DataManager.Instance.OrderLines.Add, DataManager.Instance.OrderLines.Remove);
                    _orderLines.CollectionChanged += OrderLines_CollectionChanged;
                    foreach (OrderLine line in _orderLines)
                    {
                        line.PropertyChanged += Line_PropertyChanged;
                    }
                }
                return _orderLines;
            }
        }

        public override bool IsDirty => _placedAt != null || OrderLines.Any(line => line.IsDirty);

        private void Line_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OrderLine.TotalPriceInCents):
                    RaisePropertyChanged(nameof(TotalPriceInCents));
                    break;
            }
        }

        private void OrderLines_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            void Subscribe(IList items)
            {
                foreach (INotifyPropertyChanged item in items)
                {
                    item.PropertyChanged += Line_PropertyChanged;
                }
            }
            void Unsubscribe(IList items)
            {
                foreach (INotifyPropertyChanged item in items)
                {
                    item.PropertyChanged -= Line_PropertyChanged;
                }
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        Subscribe(e.NewItems);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        Unsubscribe(e.OldItems);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (e.NewItems != null)
                    {
                        Subscribe(e.NewItems);
                    }
                    if(e.OldItems != null)
                    {
                        Unsubscribe(e.OldItems);
                    }
                    break;
            }
            RaisePropertyChanged(nameof(TotalPriceInCents));
        }

        public override void Save()
        {
            Element.Attribute(PlacedAtAttributeName)!.Value = PlacedAt.ToString();
            foreach(OrderLine orderLine in OrderLines)
            {
                orderLine.OrderId = Id;
                
            }
            OrderLines.Save();
            Rollback();
        }

        public override void Rollback()
        {
            _placedAt = null;
            foreach (OrderLine orderLine in OrderLines)
            {
                orderLine.ClearCacheAndRollback();
                orderLine.PropertyChanged -= Line_PropertyChanged;
            }
            OrderLines.Reset();
            _orderLines!.CollectionChanged -= OrderLines_CollectionChanged;
            _orderLines = null;
            RaisePropertyChanged(nameof(PlacedAt));
            RaisePropertyChanged(nameof(OrderLines));
            RaisePropertyChanged(nameof(TotalPriceInCents));
        }
    }
}
