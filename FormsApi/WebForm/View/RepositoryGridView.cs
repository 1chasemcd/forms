using System;
using FormsApi.Repository;

namespace FormsApi.WebForm.View;

public sealed class RepositoryGridView : GridView
{
    public RepositoryType RepositoryType { get; init; }
}
