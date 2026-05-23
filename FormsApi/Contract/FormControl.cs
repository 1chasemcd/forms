using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract;

public sealed record FormControl(
    [Required] string PropertyName,
    int? Width = null
);
