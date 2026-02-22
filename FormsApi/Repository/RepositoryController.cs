using FormsApi.Form.Primitives;
using FormsApi.Repository.Query;
using FormsApi.Repository.Service;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Repository;

[ApiController]
[Route("api/[controller]")]
public class RepositoryController(RepositoryServiceBuilder builder) : ControllerBase
{
    [HttpPost("get/{type}")]
    public async Task<ActionResult<IEnumerable<object>>> GetAsync(
        [FromRoute] RepositoryType type,
        [FromBody] QueryCriteria criteria)
    {
        IReadableRepositoryService service = builder.BuildWithType(type);
        IEnumerable<object>? result = await service.GetAsync(criteria);
        if (result is null)
            return NotFound();
        return Ok(result);
    }
    [HttpPost("getnew/{type}")]
    public async Task<ActionResult<object>> GetNewAsync([FromRoute] RepositoryType type)
    {
        IReadableRepositoryService service = builder.BuildWithType(type);
        object result = await service.GetNewAsync();
        return Ok(result);
    }

    [HttpPost("save/{type}")]
    public async Task<ActionResult> SaveAsync([FromRoute] RepositoryType type, [FromBody] object obj)
    {
        IWriteableRepositoryService service = builder.BuildWithTypeAndObject(type, obj);
        await service.SaveAsync();
        return NoContent();
    }

    [HttpPost("delete/{type}")]
    public async Task<ActionResult> DeleteAsync(
        [FromRoute] RepositoryType type,
        [FromBody] object obj)
    {
        IWriteableRepositoryService service = builder.BuildWithTypeAndObject(type, obj);
        await service.DeleteAsync();
        return NoContent();
    }
}
