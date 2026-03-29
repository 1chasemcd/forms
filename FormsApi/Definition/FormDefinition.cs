using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace FormsApi.Definition;

public sealed class FormDefinition
{
    [Required]
    public required SerializedType Type { get; init; }
    [Required]
    public required IViewDefinition View { get; init; }
}
