namespace FormsApi.Builder;

internal sealed class FormBuilderValidationException<TModel> : Exception
{
    public string ErrorField { get; }
    internal FormBuilderValidationException(string errorField, string message) : base(message)
    {
        ErrorField = errorField;
    }
}
