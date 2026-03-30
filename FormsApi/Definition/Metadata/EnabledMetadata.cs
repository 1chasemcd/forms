using System;
using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed record class EnabledMetadata : BaseMetadataDefinition
{
    [Required]
    public required PropertyOrConstant Enabled { get; init; }

}
