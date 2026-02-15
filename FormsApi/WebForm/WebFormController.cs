using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.WebForm;

[Route("api/[controller]")]
[ApiController]
public sealed class WebFormController : ControllerBase
{
    [HttpGet]
    public ActionResult<WebForm> GetForm(string path)
    {
        return null!;
    }
}
