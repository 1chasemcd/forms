using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract;

public sealed record FormControlLayoutDto(
    [Required] string PropertyName,
    int? Width = null
);
