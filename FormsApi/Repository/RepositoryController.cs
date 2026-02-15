using FormsApi.Repository.Query;
using FormsApi.Repository.Service;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Repository;

[ApiController]
[Route("api/[controller]")]
public class RepositoryController(RepositoryServiceBuilder builder) : ControllerBase
{
    [HttpPost("get")]
    public async Task<ActionResult<IEnumerable<object>>> GetAsync(RepositoryType type, QueryCriteria criteria)
    {
        var service = builder.BuildWithType(type);
        var result = await service.GetAsync(criteria);
        if (result is null)
            return NotFound();
        return Ok(result);
    }
    [HttpPost("getnew")]
    public async Task<ActionResult<object>> GetNewAsync(RepositoryType type)
    {
        var service = builder.BuildWithType(type);
        var result = await service.GetNewAsync();
        return Ok(result);
    }

    [HttpPost("save")]
    public async Task<ActionResult> SaveAsync(RepositoryType type, [FromBody] object obj)
    {
        var service = builder.BuildWithTypeAndObject(type, obj);
        await service.SaveAsync();
        return NoContent();
    }

    [HttpPost("delete")]
    public async Task<ActionResult> DeleteAsync(RepositoryType type, object obj)
    {
        var service = builder.BuildWithTypeAndObject(type, obj);
        await service.DeleteAsync();
        return NoContent();
    }
}
