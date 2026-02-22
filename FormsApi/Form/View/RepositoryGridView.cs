using FormsApi.Form.Primitives;

namespace FormsApi.Form.View;

public sealed record class RepositoryGridView : GridView
{
    public RepositoryType RepositoryType { get; init; }
}
