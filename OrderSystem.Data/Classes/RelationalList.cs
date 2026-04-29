using OrderSystem.Data.Models;
using System.Collections;
using System.Collections.Specialized;

namespace OrderSystem.Data.Classes
{
    public class RelationalList<T>(IEnumerable<T> originalItems, Action<T> saveAddition, Action<T> saveRemoval) : IDataCollection<T>, INotifyCollectionChanged
        where T : BaseModel
    {
        private readonly List<T> _list = [.. originalItems];

        private readonly List<T> _scheduledForRemoval = [];

        private readonly List<T> _scheduledForAdd = [];
        
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public void Add(T newItem)
        {
            _scheduledForAdd.Add(newItem);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem));
        }

        public void AddRange(IEnumerable<T> items)
        {
            _scheduledForAdd.AddRange(items);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        public void Remove(T oldItem)
        {
            if (!this.Any())
            {
                return;
            }
            int idx = this.Index().FirstOrDefault(i => ReferenceEquals(oldItem, i.Item2), (Index: -1, this.First())).Index;
            if (_list.Contains(oldItem))
            {
                _scheduledForRemoval.Add(oldItem);
            }
            else
            {
                _scheduledForAdd.Remove(oldItem);
            }
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, idx));
        }

        public void RemoveRange(IEnumerable<T> oldItems)
        {
            if(!this.Any())
            {
                return;
            }
            int idx = this.Index().FirstOrDefault(i => oldItems.Any(i2 => ReferenceEquals(i2, i.Item2)), (Index: -1, this.First())).Index;
            if (idx == -1)
            {
                return;
            }
            foreach(T item in oldItems)
            {
                if (_list.Contains(item))
                {
                    _scheduledForRemoval.Add(item);
                }
                else
                {
                    _scheduledForAdd.Remove(item);
                }
            }
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems.ToList(), idx));
        }

        internal void Reset()
        {
            foreach(T item in _list)
            {
                item.Rollback();
            }
            _scheduledForRemoval.Clear();
            _scheduledForAdd.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        internal void Save()
        {
            foreach(T item in _list)
            {
                item.Save();
            }
            foreach (T item in _scheduledForAdd)
            {
                item.Save();
                saveAddition(item);
            }
            _scheduledForAdd.Clear();
            foreach(T item in _scheduledForRemoval)
            {
                saveRemoval(item);
            }
        }

        public IEnumerator<T> GetEnumerator() => _list.Where(i => !_scheduledForRemoval.Any(r => ReferenceEquals(r, i))).Concat(_scheduledForAdd).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
