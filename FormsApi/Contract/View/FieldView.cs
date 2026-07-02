using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.View;

public sealed record FieldView : View
{
    [Required]
    public required IReadOnlyList<FormFieldInfoContainer> Fields { get; init; }
}
