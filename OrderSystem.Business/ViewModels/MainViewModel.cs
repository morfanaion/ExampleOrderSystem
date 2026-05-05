using OrderSystem.Business.Classes;
using OrderSystem.Business.Resources;
using System.Windows.Input;

namespace OrderSystem.Business.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public List<ViewModel> Tabs { get; }

        public override string ViewTitle => OrderBusinessResources.OrderSystemTitle;

        public ICommand CloseCommand { get; set; }

        public MainViewModel(List<ViewModel> tabs)
        {
            CloseCommand = new RelayCommand(Commit);
            Tabs = tabs;
        }
    }
}
