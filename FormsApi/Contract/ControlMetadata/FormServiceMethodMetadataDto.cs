using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public sealed class FormServiceMethodMetadataDto : IControlMetadataDto
{
    [Required]
    public required FormServiceMethodDto Value { get; init; }
}
