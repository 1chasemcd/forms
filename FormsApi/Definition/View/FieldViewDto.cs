using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Field;

namespace FormsApi.Definition.View;

public sealed record class FieldViewDto : BaseViewDto
{
    [Required]
    public required IEnumerable<FieldDto> Fields { get; init; }
}
