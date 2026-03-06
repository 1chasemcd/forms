namespace FormsApi.Form.Field;

public sealed record class Button : BaseField
{
    public override string Id => MethodToRunOnChange ?? Guid.NewGuid().ToString();
}
