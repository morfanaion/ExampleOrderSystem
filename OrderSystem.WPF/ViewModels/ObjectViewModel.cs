using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using OrderSystem.WPF.Classes;
using OrderSystem.WPF.Resources;
using OrderSystem.WPF.Services;
using System.Windows.Input;

namespace OrderSystem.WPF.ViewModels
{
    internal abstract class ObjectViewModel<T> : ViewModel where T: BaseModel
    {
        public ICommand CancelCommand { get; }
        public ICommand OkCommand { get; }
        public IValidator Validation { get; }
        public T CurrentObject { get; set; }

        public ObjectViewModel(T currentObject, IValidator validator)
        {
            CancelCommand = new RelayCommand(Discard);
            OkCommand = new RelayCommand(ExecuteOkCommand);
            Validation = validator;
            CurrentObject = currentObject;
        }

        private void ExecuteOkCommand()
        {
            switch (Validation.Validate())
            {
                case ValidationLevel.Error:
                    ServiceLocator.GetService<IUiService>().Alert(WpfOrderResources.InvalidInput, WpfOrderResources.InvalidInputMessage, this);
                    return;
                case ValidationLevel.Warning:
                    if (ServiceLocator.GetService<IUiService>().Confirm(WpfOrderResources.InputWarning, WpfOrderResources.InputWarningMessage, this))
                    {
                        break;
                    }
                    return;
            }
            Commit();
        }

        internal override bool CancelAllowed()
        {
            if (!CurrentObject.IsDirty)
            {
                return true;
            }
            return ServiceLocator.GetService<IUiService>().Confirm(WpfOrderResources.Cancel, WpfOrderResources.CancelPendingChangesQuestion, this);
        }
    }
}
