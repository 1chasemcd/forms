using FormsApi.Form.Primitives;

namespace FormsApi.Form.View;

public sealed record class RepositoryGridView : GridView
{
    public required RepositoryType RepositoryType { get; init; }
}
