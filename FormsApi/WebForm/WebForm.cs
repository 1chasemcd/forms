using System;
using FormsApi.Repository;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace FormsApi.WebForm;

public class WebForm
{
    public RepositoryType Type { get; init; }
    public required IView View { get; init; }
}
