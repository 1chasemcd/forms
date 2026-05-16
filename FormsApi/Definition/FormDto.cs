using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Metadata;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace FormsApi.Definition;

public sealed class FormDto
{
    [Required]
    public required TypeDto ModelType { get; init; }
    [Required]
    public required BaseViewDto View { get; init; }
    [Required]
    public required IEnumerable<ModelMetadataDto> ModelMetadatas { get; init; }
}
