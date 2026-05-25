using System.ComponentModel.DataAnnotations;
using FormsApi.Contract.MetadataContainer;

namespace FormsApi.Contract;

public sealed record FormResponse
{
    [Required]
    public required TypeDto ModelType { get; init; }
    [Required]
    public required IReadOnlyList<View.View> Views { get; init; }
    [Required]
    public required ICollection<ModelMetadataContainer> ModelMetadatas { get; init; }
}
