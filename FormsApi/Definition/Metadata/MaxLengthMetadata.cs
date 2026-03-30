using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed record class MaxLengthMetadata : BaseMetadataDefinition
{
    [Required]
    public required PropertyOrConstant MaxLength { get; init; }
}
