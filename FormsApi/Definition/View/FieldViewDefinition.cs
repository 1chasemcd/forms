using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Field;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.View;

public sealed record class FieldViewDefinition : BaseViewDefinition
{
    [Required]
    public required IEnumerable<FieldDefinition> Fields { get; init; }
}
