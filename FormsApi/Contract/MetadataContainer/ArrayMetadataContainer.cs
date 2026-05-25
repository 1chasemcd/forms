using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataContainer;

public sealed record ArrayMetadataContainer : PropertyMetadataContainer
{
    public ArrayMetadataContainer(TypeDto enumeratedType)
    {
        EnumeratedType = enumeratedType;
    }

    [Required]
    public TypeDto EnumeratedType { get; init; }
}
