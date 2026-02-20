using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Form;

[Route("api/[controller]")]
[ApiController]
public sealed class FormController : ControllerBase
{
    [HttpGet]
    public ActionResult<FormModel> GetForm(string path)
    {
        return null!;
    }
}
