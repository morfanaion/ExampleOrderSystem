using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using OrderSystem.WPF.Classes;
using OrderSystem.WPF.Orchestration.Interfaces;
using System.Collections.Specialized;

namespace OrderSystem.WPF.ViewModels.ProductGroups
{
    internal class ProductGroupsViewModel : ViewModel
    {
        private readonly IProductGroupOrchestrator _orchestrator;

        public RelayCommand AddCommand { get; }
        public RelayCommand<ProductGroup> EditCommand { get; }
        public RelayCommand<ProductGroup> DeleteCommand { get; }

        public ProductGroupsViewModel(IProductGroupOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            AddCommand = new RelayCommand(() => _orchestrator.AddNewProductGroup(this));
            EditCommand = new RelayCommand<ProductGroup>(productGroup => _orchestrator.EditProductGroup(productGroup!, this), product => product is not null);
            DeleteCommand = new RelayCommand<ProductGroup>(productGroup => _orchestrator.DeleteProductGroup(productGroup!, this), product => product is not null);
            if (_orchestrator.ProductGroupsSource is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += ProductGroupsChanged;
            }
        }

        public override void Dispose()
        {
            if (_orchestrator.ProductGroupsSource is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged -= ProductGroupsChanged;
            }
            base.Dispose();
        }

        private void ProductGroupsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(ProductGroupsSource));
        }

        public IEnumerable<ProductGroup> ProductGroupsSource => _orchestrator.ProductGroupsSource.Where(pg => !pg.IsExpired);

        public override string ViewTitle => OrderResources.ProductGroups;
    }
}
