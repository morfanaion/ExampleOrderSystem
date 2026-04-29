using OrderSystem.WPF.Classes;
using OrderSystem.WPF.Resources;

namespace OrderSystem.WPF.ViewModels
{
    internal class MainViewModel : ViewModel
    {
        public List<ViewModel> Tabs { get; }

        public override string ViewTitle => WpfOrderResources.OrderSystemTitle;

        public RelayCommand CloseCommand { get; set; }

        public MainViewModel(List<ViewModel> tabs)
        {
            CloseCommand = new RelayCommand(Commit);
            Tabs = tabs;
        }
    }
}
