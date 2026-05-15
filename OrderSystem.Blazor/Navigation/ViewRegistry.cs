using OrderSystem.Business.ViewModels;

namespace OrderSystem.Blazor.Navigation
{
    public static class ViewRegistry
    {
        private static readonly Dictionary<Type, Type> _map = new();

        public static void Register<TVm, TComponent>()
        {
            _map[typeof(TVm)] = typeof(TComponent);
        }

        public static Type Resolve(ViewModel vm)
        {
            return _map[vm.GetType()];
        }
    }
