using OrderSystem.Data.Models;
using OrderSystem.Data.Validation;
using OrderSystem.Business.Classes;
using OrderSystem.Business.Resources;
using OrderSystem.Business.Services;
using System.Windows.Input;

namespace OrderSystem.Business.ViewModels
{
    public abstract class ObjectViewModel<T> : ViewModel where T: BaseModel
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
                    ServiceLocator.GetService<IUiService>().Alert(OrderBusinessResources.InvalidInput, OrderBusinessResources.InvalidInputMessage, this);
                    return;
                case ValidationLevel.Warning:
                    if (ServiceLocator.GetService<IUiService>().Confirm(OrderBusinessResources.InputWarning, OrderBusinessResources.InputWarningMessage, this))
                    {
                        break;
                    }
                    return;
            }
            Commit();
        }

        public override bool CancelAllowed()
        {
            if (!CurrentObject.IsDirty)
            {
                return true;
            }
            return ServiceLocator.GetService<IUiService>().Confirm(OrderBusinessResources.Cancel, OrderBusinessResources.CancelPendingChangesQuestion, this);
        }
    }
}
