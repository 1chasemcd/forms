using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Metadata;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace FormsApi.Definition;

public sealed class FormDefinition
{
    [Required]
    public required SerializedType ModelType { get; init; }
    [Required]
    public required BaseViewDefinition View { get; init; }
}
