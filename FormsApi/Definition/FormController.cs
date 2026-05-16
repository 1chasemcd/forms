using FormsApi.Definition.Service;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Definition;

[Route("api/[controller]")]
[ApiController]
public sealed class FormController(FormRegistry registry) : ControllerBase
{
    [HttpGet("{path}")]
    public ActionResult<FormDto> GetForm([FromRoute] string path)
    {
        FormDto? form = registry.TryGet(path);
        if (form is null)
            return NotFound();
        return Ok(form);
    }
}
