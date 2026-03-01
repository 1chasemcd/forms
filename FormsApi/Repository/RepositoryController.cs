using System.Text.Json;
using FormsApi.Form.Primitives;
using FormsApi.Repository.Query;
using FormsApi.Repository.Service;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Repository;

[ApiController]
[Route("api/[controller]")]
public sealed class RepositoryController(IRepositoryServiceFactory factory) : ControllerBase
{
    [HttpPost("get/{type}")]
    public async Task<ActionResult<IEnumerable<object>>> GetAsync(
        [FromRoute] RepositoryType type,
        [FromBody] QueryCriteria criteria)
    {
        IReadableRepositoryService service = factory.BuildWithType(type);
        IEnumerable<object>? result = await service.GetAsync(criteria);
        if (result is null)
            return NotFound();
        return Ok(result);
    }
    [HttpPost("getnew/{type}")]
    public async Task<ActionResult<object>> GetNewAsync([FromRoute] RepositoryType type)
    {
        IReadableRepositoryService service = factory.BuildWithType(type);
        object result = await service.GetNewAsync();
        return Ok(result);
    }

    [HttpPost("save/{type}")]
    public async Task<ActionResult> SaveAsync([FromRoute] RepositoryType type, [FromBody] JsonElement body)
    {
        IWriteableRepositoryService service = factory.BuildWithTypeAndObject(type, body);
        await service.SaveAsync();
        return NoContent();
    }

    [HttpPost("delete/{type}")]
    public async Task<ActionResult> DeleteAsync(
        [FromRoute] RepositoryType type,
        [FromBody] JsonElement body)
    {
        IWriteableRepositoryService service = factory.BuildWithTypeAndObject(type, body);
        await service.DeleteAsync();
        return NoContent();
    }
}
