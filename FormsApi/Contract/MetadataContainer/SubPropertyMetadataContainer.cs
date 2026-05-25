using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataContainer;

public sealed record SubPropertyMetadataContainer : PropertyMetadataContainer
{
    public SubPropertyMetadataContainer(TypeDto subPropertyType)
    {
        SubPropertyType = subPropertyType;
    }

    [Required]
    public TypeDto SubPropertyType { get; init; }
}
