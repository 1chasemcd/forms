using System.ComponentModel.DataAnnotations;
using FormsApi.Contract.MetadataCollection;
using FormsApi.Contract.View;

namespace FormsApi.Contract;

public sealed record FormResponse
{
    [Required]
    public required TypeDto ModelType { get; init; }
    [Required]
    public required IReadOnlyList<BaseViewDto> Views { get; init; }
    [Required]
    public required ICollection<ModelMetadataCollectionDto> ModelMetadatas { get; init; }
}
