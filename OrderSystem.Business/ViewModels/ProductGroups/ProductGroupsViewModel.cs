using OrderSystem.Business.Classes;
using OrderSystem.Business.Orchestration.Interfaces;
using OrderSystem.Data.Managers;
using OrderSystem.Data.Models;
using OrderSystem.Data.Resources;
using System.Collections.Specialized;
using System.Windows.Input;

namespace OrderSystem.Business.ViewModels.ProductGroups
{
    public class ProductGroupsViewModel : ListViewModel<ProductGroup>
    {
        private readonly IProductGroupOrchestrator _orchestrator;
        public override string ViewTitle => OrderResources.ProductGroups;
        public ICommand ClearFilterCommand { get; }

        public ProductGroupsViewModel(IProductGroupOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            AddCommand = new RelayCommand(() =>
            {
                if(_orchestrator.AddNewProductGroup(this) is ProductGroup newProductGroup)
                {
                    SelectedItem = newProductGroup;
                }
            });
            EditCommand = new RelayCommand<ProductGroup>(productGroup => _orchestrator.EditProductGroup(productGroup!, this), product => product is not null);
            DeleteCommand = new RelayCommand<ProductGroup>(productGroup => _orchestrator.DeleteProductGroup(productGroup!, this), product => product is not null);
            ClearFilterCommand = new RelayCommand(ClearFilterCommandExecute);
            DataManager.Instance.ProductGroups.CollectionChanged += ProductGroupsChanged;
        }

        private void ClearFilterCommandExecute()
        {
            SearchText = string.Empty;
        }

        public override void Dispose()
        {
            DataManager.Instance.ProductGroups.CollectionChanged -= ProductGroupsChanged;
            base.Dispose();
        }

        private void ProductGroupsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(ProductGroupsSource));
        }

        public IEnumerable<ProductGroup> ProductGroupsSource
        {
            get
            {
                IEnumerable<ProductGroup> source = _orchestrator.ProductGroupsSource;
                if (!string.IsNullOrEmpty(_searchText))
                {
                    source = source.Where(pg => pg.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase));
                }
                return source;
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(ProductGroupsSource));
                }
            }
        }
    }
}
