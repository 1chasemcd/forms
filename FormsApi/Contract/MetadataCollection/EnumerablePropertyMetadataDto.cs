using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.MetadataCollection;

public sealed record EnumerablePropertyMetadataDto([Required] TypeDto EnumeratedType) : IPropertyMetadataDto;
