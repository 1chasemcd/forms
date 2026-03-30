using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Metadata;

namespace FormsApi.Definition.Field;

public sealed record class FieldDefinition
{
    [Required]
    public FieldType Type { get; init; }
    [Required]
    public required string Property { get; init; }
    public IEnumerable<BaseMetadataDefinition>? FieldMetadatas { get; init; }
}
