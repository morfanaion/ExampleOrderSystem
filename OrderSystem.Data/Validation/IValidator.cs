namespace OrderSystem.Data.Validation
{
    public interface IValidator
    {
        ValidationLevel Validate();
        ValidationState this[string propertyName] { get; }
    }
}
