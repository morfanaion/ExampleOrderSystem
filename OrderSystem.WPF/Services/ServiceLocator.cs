namespace OrderSystem.WPF.Services
{
    internal class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = [];

        public static void RegisterService<TInterface, TService>(TService service)
            where TService: class, TInterface
        {
            _services.Add(typeof(TInterface), service);
        }

        public static TInterface GetService<TInterface>() => (TInterface)_services[typeof(TInterface)];
    }
}
