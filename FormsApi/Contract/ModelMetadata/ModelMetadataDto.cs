using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ModelMetadata;

public sealed class ModelMetadataDto
{
    [Required]
    public required TypeDto Type { get; init; }
    [Required]
    public required Dictionary<string, IPropertyMetadataDto> PropertyMetadatas { get; init; }
}
