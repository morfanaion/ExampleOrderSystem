using OrderSystem.Data.Models;
using System.Windows.Input;

namespace OrderSystem.Business.ViewModels
{
    public abstract class ListViewModel<T>() : ViewModel where T : BaseModel
    {
        private T? _selectedItem;
        public T? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                RaisePropertyChanged();
            }
        }
        public ICommand AddCommand { get; protected set; } = null!;
        public ICommand EditCommand { get; protected set; } = null!;
        public ICommand DeleteCommand { get; protected set; } = null!;

        private ICommand? _itemDoubleClickCommand = null;
        public ICommand ItemDoubleClickCommand
        {
            get => _itemDoubleClickCommand ?? EditCommand;
            set
            {
                if(_itemDoubleClickCommand != value)
                {
                    _itemDoubleClickCommand = value;
                    RaisePropertyChanged(nameof(ItemDoubleClickCommand));
                }
            }
        }
    }
}
