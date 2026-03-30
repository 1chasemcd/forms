using System;
using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed record class RequiredMetadata : BaseMetadataDefinition
{
    [Required]
    public required PropertyOrConstant Required { get; init; }
}
