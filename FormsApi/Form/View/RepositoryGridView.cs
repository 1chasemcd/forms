using System.ComponentModel.DataAnnotations;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.View;

public sealed record class RepositoryGridView : GridView
{
    [Required]
    public required SerializedType RepositoryType { get; init; }
}
