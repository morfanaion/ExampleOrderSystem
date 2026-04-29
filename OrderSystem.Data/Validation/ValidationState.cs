using OrderSystem.Data.Classes;

namespace OrderSystem.Data.Validation
{
    public class ValidationState : BaseNotifier
    {
        private ValidationLevel _validationLevel;
        public ValidationLevel Level
        {
            get => _validationLevel;
            set
            {
                _validationLevel = value;
                RaisePropertyChanged();
            }
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }

    }
}
