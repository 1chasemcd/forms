using System.ComponentModel.DataAnnotations;
using FormsApi.Form.Primitives;
using FormsApi.Form.View;

namespace FormsApi.Form;

public sealed class FormDefinition
{
    [Required]
    public required SerializedType Type { get; init; }
    [Required]
    public required BaseView View { get; init; }
}
