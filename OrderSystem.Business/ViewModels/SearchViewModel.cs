using OrderSystem.Business.Classes;
using OrderSystem.Data.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace OrderSystem.Business.ViewModels
{
    public abstract class SearchViewModel : ViewModel
    {
        private readonly string _viewTitle;

        public ICommand CancelCommand { get; }
        public ICommand OkCommand { get; }
        public ViewModel InnerViewModel { get; }

        public override string ViewTitle => _viewTitle;

        public SearchViewModel(string viewTitle, ViewModel innerViewModel)
        {
            _viewTitle = viewTitle;
            InnerViewModel = innerViewModel;
            OkCommand = new RelayCommand(Commit, OkCommandCanExecute);
            CancelCommand = new RelayCommand(Discard);
        }

        protected abstract bool OkCommandCanExecute();
    }

    public class SearchViewModel<T> : SearchViewModel where T : BaseModel
    {
        public new ListViewModel<T> InnerViewModel { get => (ListViewModel<T>)base.InnerViewModel; }
        
        public SearchViewModel(string viewTitle, ListViewModel<T> innerViewModel) : base(viewTitle, innerViewModel)
        {
            innerViewModel.ItemDoubleClickCommand = OkCommand;
            innerViewModel.PropertyChanged += InnerViewModel_PropertyChanged;
        }

        private void InnerViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(InnerViewModel.SelectedItem))
            {
                ((RelayCommand)OkCommand).RaiseCanExecuteChanged();
            }
        }

        public override void Dispose()
        {
            InnerViewModel.PropertyChanged -= InnerViewModel_PropertyChanged;
            base.Dispose();
        }

        protected override bool OkCommandCanExecute() => InnerViewModel.SelectedItem != null;
    }
}
