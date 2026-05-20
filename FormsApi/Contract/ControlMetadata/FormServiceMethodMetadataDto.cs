using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public sealed record FormServiceMethodMetadataDto : BaseControlMetadataDto
{
    [Required]
    public required FormServiceMethodDto Value { get; init; }
}
