using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataContainer;

public sealed record ArrayMetadataContainer([Required] TypeDto EnumeratedType) : PropertyMetadataContainer;
