using FormsApi.Form.Primitives;

namespace FormsApi.Form.Field;

public sealed record class StaticTextField : BaseField
{
    private readonly string _id = Guid.NewGuid().ToString();
    public override string Id => _id;
    public required PropertyOrConstant Text { get; init; }
}
