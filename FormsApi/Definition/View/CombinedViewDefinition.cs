using System.ComponentModel.DataAnnotations;

namespace FormsApi.Definition.View;

public sealed record class CombinedViewDefinition : BaseViewDefinition
{
    [Required]
    public required IEnumerable<BaseViewDefinition> Views { get; init; }
}
