using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataContainer;

public sealed record SubPropertyMetadataContainer([Required] TypeDto SubPropertyType) : PropertyMetadataContainer;