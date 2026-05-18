using System.ComponentModel.DataAnnotations;
using FormsApi.Contract.MetadataCollection;
using FormsApi.Contract.View;

namespace FormsApi.Contract;

public sealed class FormDto
{
    [Required]
    public required TypeDto ModelType { get; init; }
    [Required]
    public required BaseViewDto View { get; init; }
    [Required]
    public required IEnumerable<ModelMetadataCollectionDto> ModelMetadatas { get; init; }
}
