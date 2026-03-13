using System;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.Field;

public sealed record class ButtonField : BaseField
{
    public required OnChangeEvent OnChange { get; init; }
    public PropertyOrConstant? Disabled { get; init; }
}
