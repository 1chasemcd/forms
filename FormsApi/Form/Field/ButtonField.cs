using System;
using System.ComponentModel.DataAnnotations;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.Field;

public sealed record class ButtonField : BaseField
{
    [Required]
    public required RecalculateEvent RecalculateEvent { get; init; }
    public PropertyOrConstant? Disabled { get; init; }
}
