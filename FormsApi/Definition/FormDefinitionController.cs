using FormsApi.Common.Registry;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Definition;

[Route("api/[controller]")]
[ApiController]
public sealed class FormDefinitionController(FormRegistry registry) : ControllerBase
{
    [HttpGet("{path}")]
    public ActionResult<FormDefinition> GetForm([FromRoute] string path)
    {
        FormDefinition? form = registry.TryGet(path);
        if (form is null)
            return NotFound();
        return Ok(form);
    }
}
