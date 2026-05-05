using OrderSystem.Business.ViewModels;
using System.Runtime.CompilerServices;
using System.Windows;

namespace OrderSystem.WPF.Navigation
{
    internal static class HostRegistry
    {
        private static readonly ConditionalWeakTable<object, FrameworkElement> _map = [];

        public static void Register(ViewModel vm, FrameworkElement view)
        {
            _map.Remove(vm);
            _map.Add(vm, view);
        }

        public static bool TryGetView(ViewModel vm, out FrameworkElement? view) => _map.TryGetValue(vm, out view);
    }
}
