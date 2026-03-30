using System;
using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Metadata;

public sealed record class LabelMetadata : BaseMetadataDefinition
{
    [Required]
    public required PropertyOrConstant Label { get; init; }

}
