using System.Text.Json;
using FormsApi.Definition.Primitives;
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
        [FromRoute] SerializedType type,
        [FromBody] QueryCriteria criteria)
    {
        IRepositoryCallable service = factory.BuildQueryService(type, criteria);
        if (await service.Invoke() is not IEnumerable<object> result)
            return NotFound();
        return Ok(result);
    }
    [HttpPost("getnew/{type}")]
    public async Task<ActionResult<object>> CreateAsync([FromRoute] SerializedType type)
    {
        IRepositoryCallable service = factory.BuildCreateService(type);
        object result = await service.Invoke();
        return Ok(result);
    }

    [HttpPost("save/{type}")]
    public async Task<ActionResult> SaveAsync([FromRoute] SerializedType type, [FromBody] JsonElement body)
    {
        object obj = body.Deserialize(type.GetRuntimeType()) ?? throw new InvalidOperationException("Could not deserialize to type");
        IRepositoryCallable service = factory.BuildSaveService(type, obj);
        await service.Invoke();
        return NoContent();
    }

    [HttpPost("delete/{type}")]
    public async Task<ActionResult> DeleteAsync(
        [FromRoute] SerializedType type,
        [FromBody] JsonElement body)
    {
        object obj = body.Deserialize(type.GetRuntimeType()) ?? throw new InvalidOperationException("Could not deserialize to type");
        IRepositoryCallable service = factory.BuildDeleteService(type, obj);
        await service.Invoke();
        return NoContent();
    }
}
