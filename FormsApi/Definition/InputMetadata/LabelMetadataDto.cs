using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.InputMetadata;

public sealed class LabelMetadataDto : IInputMetadataDto
{
    [Required]
    public required PropertyOrConstantDto Value { get; init; }
}
