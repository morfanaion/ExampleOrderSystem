namespace OrderSystem.Business.Classes
{
    public class SearchProvider
    {
        private Func<object?> _findObject;
        public SearchProvider(Func<object?> findObject)
        {
            _findObject = findObject;
        }

        public object? Search()
        {
            return _findObject();
        }
    }

    public class SearchProvider<T> : SearchProvider
    {
        public SearchProvider(Func<T> findObject) : base(() => findObject())
        {
        }

        public new T? Search()
        {
            return (T?)base.Search();
        }
    }
}
