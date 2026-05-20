using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataCollection;

public sealed record SubPropertyMetadataDto([Required] TypeDto SubPropertyType) : IPropertyMetadataDto;