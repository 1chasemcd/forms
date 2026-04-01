using System;
using System.ComponentModel.DataAnnotations;

namespace FormsApi.Definition.Metadata;

public sealed record class MetadataDefinition
{
    [Required]
    public MetadataType Type { get; init; }
    [Required]
    public required object Value { get; init; }
}
