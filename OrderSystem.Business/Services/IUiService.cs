using OrderSystem.Business.ViewModels;

namespace OrderSystem.Business.Services
{
    public interface IUiService
    {
        void Alert(string header, string message, ViewModel? owner = null);
        bool Confirm(string header, string question, ViewModel? owner = null);
        bool Ask(string header, string question, ViewModel? owner = null);
        void Show(ViewModel viewModel);
        bool ShowDialog(ViewModel viewModel, ViewModel? owner = null);
    }
}
