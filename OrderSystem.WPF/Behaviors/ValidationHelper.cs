using OrderSystem.Data.Validation;
using System.Windows;

namespace OrderSystem.WPF.Behaviors
{
    public static class ValidationHelper
    {
        public static readonly DependencyProperty ValidationStateProperty =
            DependencyProperty.RegisterAttached(
                "ValidationState",
                typeof(ValidationState),
                typeof(ValidationHelper),
                new PropertyMetadata(null));

        public static void SetValidationState(DependencyObject element, ValidationState value)
            => element.SetValue(ValidationStateProperty, value);

        public static ValidationState GetValidationState(DependencyObject element)
            => (ValidationState)element.GetValue(ValidationStateProperty);
    }
}
