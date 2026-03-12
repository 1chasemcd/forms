using System.ComponentModel.DataAnnotations;

namespace FormsApi.Form.View;

public sealed record class SubPropertyGridView : GridView
{
    [Required]
    public required string SubPropertyName { get; init; }
}
