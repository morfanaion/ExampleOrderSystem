using OrderSystem.Data.Classes;
using OrderSystem.Data.Models;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Xml.Linq;

namespace OrderSystem.Data.Managers
{
    public abstract class BaseManager<T> : IDataCollection<T>, INotifyCollectionChanged where T : BaseModel
    {
        internal BaseManager(XElement element)
        {
            _element = element;
            _items = [.. element.Elements().Select(e => (T)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, [e], null)!)];
        }

        private XElement _element;
        protected readonly List<T> _items;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public void Add(T item)
        {
            _items.Add(item);
            _element.Add(item.Element);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public virtual void Remove(T item)
        {
            int idx = _items.IndexOf(item);
            _items.Remove(item);
            item.Element.Remove();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, idx));
        }

        protected void RaiseReplace(T oldItem, T newItem)
        {
            int idx = _items.IndexOf(oldItem);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, idx));
        }

        protected void SetAttribute(string attributeName, string value) => _element.Attribute(attributeName)!.Value = value;

        protected TValue GetAttribute<TValue>(string attributeName, Func<string, TValue> convertValue) => convertValue(_element.Attribute(attributeName)!.Value);

        protected string GetAttribute(string attributeName) => _element.Attribute(attributeName)!.Value;

        public abstract T CreateNew();

        public abstract T CreateDummy();

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
