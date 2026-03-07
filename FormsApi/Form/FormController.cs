using FormsApi.Common.Registry;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Form;

[Route("api/[controller]")]
[ApiController]
public sealed class FormController(FormRegistry registry) : ControllerBase
{
    [HttpGet("{path}")]
    public ActionResult<BaseForm> GetForm([FromRoute] string path)
    {
        BaseForm? form = registry.TryGet(path);
        if (form is null)
            return NotFound();
        return Ok(form);
    }
}
