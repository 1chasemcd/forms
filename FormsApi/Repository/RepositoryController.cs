using System.Collections;
using System.Text.Json;
using FormsApi.Definition.Primitives;
using FormsApi.Repository.Service;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Repository;

[ApiController]
[Route("api/[controller]")]
public sealed class RepositoryController(IRepositoryServiceFactory factory) : ControllerBase
{
    [HttpPost("getall/{type}")]
    public async Task<IActionResult> GetAllAsync([FromRoute] SerializedType type)
    {
        IRepositoryCallable service = factory.BuildQueryService(type);

        if (await service.Invoke() is not IEnumerable result)
            return NotFound();
        return SerializeResult(result.Cast<object>().ToList());
    }
    [HttpPost("get/{type}/{id}")]
    public async Task<IActionResult> GetAsync(
    [FromRoute] SerializedType type, [FromRoute] string id)
    {
        IRepositoryCallable service = factory.BuildQueryService(type, id);

        if (await service.Invoke() is not { } result)
            return NotFound();
        return SerializeResult(result);
    }

    [HttpPost("getnew/{type}")]
    public async Task<IActionResult> CreateAsync([FromRoute] SerializedType type)
    {
        IRepositoryCallable service = factory.BuildCreateService(type);
        if (await service.Invoke() is not { } result)
            return Problem();
        return SerializeResult(result);
    }

    [HttpPost("save/{type}")]
    public async Task<IActionResult> SaveAsync([FromRoute] SerializedType type, [FromBody] JsonElement body)
    {
        if (body.Deserialize(type.GetRuntimeType()) is not { } obj)
            return Problem();
        IRepositoryCallable service = factory.BuildSaveService(type, obj);
        await service.Invoke();
        return NoContent();
    }

    [HttpPost("delete/{type}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] SerializedType type,
        [FromBody] JsonElement body)
    {
        if (body.Deserialize(type.GetRuntimeType()) is not { } obj)
            return Problem();
        IRepositoryCallable service = factory.BuildDeleteService(type, obj);
        await service.Invoke();
        return NoContent();
    }

    private static JsonResult SerializeResult(object data)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null
        };

        return new JsonResult(data, options);
    }
}
