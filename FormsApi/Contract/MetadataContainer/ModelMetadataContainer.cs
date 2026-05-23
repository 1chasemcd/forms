using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataContainer;

public sealed record ModelMetadataContainer
{
    [Required]
    public required TypeDto Type { get; init; }
    [Required]
    public required Dictionary<string, PropertyMetadataContainer> PropertyMetadatas { get; init; }
}
