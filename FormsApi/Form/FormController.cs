using FormsApi.Common.Registry;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Form;

[Route("api/[controller]")]
[ApiController]
public sealed class FormController(FormRegistry registry) : ControllerBase
{
    [HttpGet("{path}")]
    public ActionResult<FormModel> GetForm([FromRoute] string path)
    {
        FormModel? form = registry.TryGet(path);
        if (form is null)
            return NotFound();
        return form;
    }
}
