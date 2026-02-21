using FormsApi.Common.Registry;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Form;

[Route("api/[controller]")]
[ApiController]
public sealed class FormController(FormRegistry registry) : ControllerBase
{
    [HttpGet]
    public ActionResult<FormModel> GetForm(string path)
    {
        FormModel? form = registry.Get(path);
        if (form is null)
            return NotFound();
        return form;
    }
}
