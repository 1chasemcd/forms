using System;
using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed record class VisibleMetadata : BaseMetadataDefinition
{
    [Required]
    public required PropertyOrConstant Visible { get; init; }
}
