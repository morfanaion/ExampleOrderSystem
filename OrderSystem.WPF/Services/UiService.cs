using OrderSystem.Business.Services;
using OrderSystem.Business.ViewModels;
using OrderSystem.WPF.Navigation;
using System.ComponentModel;
using System.Windows;

namespace OrderSystem.WPF.Services
{
    internal class UiService : IUiService
    {
        public void Show(ViewModel viewModel)
        {
            Window theWindow = CreateWindow(viewModel);
            viewModel.Finish += Close;
            theWindow.Show();

            void Close(bool? result)
            {
                viewModel.Finish -= Close;
                theWindow.Close();
                theWindow.DataContext = null;
            }
        }

        public static Window CreateWindow(ViewModel viewModel)
        {
            Window theWindow = new BaseWindow();
            theWindow.SetValue(UiScope.UiScopeProperty, new UiScope(theWindow));
            theWindow.DataContext = viewModel;
            return theWindow;
        }

        public bool ShowDialog(ViewModel viewModel, ViewModel? owner = null)
        {
            Window ownerWindow;
            if (owner != null && HostRegistry.TryGetView(owner, out FrameworkElement? frameworkElement))
            {
                ownerWindow = UiScope.GetScope(frameworkElement!).RootWindow;
            }
            else
            {
                ownerWindow = Application.Current.MainWindow;
            }
            bool dialogResult = false;
            Window theWindow = CreateWindow(viewModel);
            viewModel.Finish += Close;
            theWindow.Closing += Closing;
            theWindow.Owner = ownerWindow;
            _ = theWindow.ShowDialog();
            return dialogResult;

            void Close(bool? result)
            {
                dialogResult = result ?? false;
                theWindow.Dispatcher.Invoke(() =>
                {
                    theWindow.Close();
                });
            }

            void Closing(object? sender, CancelEventArgs e)
            {
                if(!dialogResult && !viewModel.CancelAllowed())
                {
                    e.Cancel = true;
                }
                else
                {
                    theWindow.DataContext = null;
                    viewModel.Finish -= Close;
                    theWindow.Closing -= Closing;
                }
            }
        }

        public bool Confirm(string header, string question, ViewModel? owner = null) => Ask(header, question, MessageBoxImage.Warning, owner);

        private static bool Ask(string header, string question, MessageBoxImage image, ViewModel? owner)
        {
            MessageBoxResult result;
            if (owner != null && HostRegistry.TryGetView(owner, out FrameworkElement? frameworkElement))
            {
                Window ownerWindow = UiScope.GetScope(frameworkElement!).RootWindow;
                result = MessageBox.Show(ownerWindow, question, header, MessageBoxButton.YesNo, image);
            }
            else
            {
                result = MessageBox.Show(question, header, MessageBoxButton.YesNo, image);
            }
            return result == MessageBoxResult.Yes;
        }

        public bool Ask(string header, string question, ViewModel? owner = null) => Ask(header, question, MessageBoxImage.Question, owner);

        public void Alert(string header, string message, ViewModel? owner = null)
        {
            if (owner != null && HostRegistry.TryGetView(owner, out FrameworkElement? frameworkElement))
            {
                Window ownerWindow = UiScope.GetScope(frameworkElement!).RootWindow;
                MessageBox.Show(ownerWindow, message, header, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(message, header, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
