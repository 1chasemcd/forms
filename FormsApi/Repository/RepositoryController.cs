using System.Text.Json;
using FormsApi.Definition.Primitives;
using FormsApi.Repository.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace FormsApi.Repository;

[ApiController]
[Route("api/[controller]")]
public sealed class RepositoryController(IRepositoryServiceFactory factory) : ControllerBase
{
    [HttpPost("query/{type}")]
    public async Task<IActionResult> QueryAsync(
        [FromRoute] SerializedType type, ODataQueryOptions options)
    {
        IRepositoryCallable service = factory.BuildQueryService(type, "");

        if (await service.Invoke() is not IQueryable result)
            return NotFound();
        return SerializeResult(options.ApplyTo(result));
    }

    [HttpPost("getnew/{type}")]
    public async Task<IActionResult> CreateAsync([FromRoute] SerializedType type)
    {
        IRepositoryCallable service = factory.BuildCreateService(type);
        object result = await service.Invoke();
        return SerializeResult(result);
    }

    [HttpPost("save/{type}")]
    public async Task<IActionResult> SaveAsync([FromRoute] SerializedType type, [FromBody] JsonElement body)
    {
        object obj = body.Deserialize(type.GetRuntimeType()) ?? throw new InvalidOperationException("Could not deserialize to type");
        IRepositoryCallable service = factory.BuildSaveService(type, obj);
        await service.Invoke();
        return NoContent();
    }

    [HttpPost("delete/{type}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] SerializedType type,
        [FromBody] JsonElement body)
    {
        object obj = body.Deserialize(type.GetRuntimeType()) ?? throw new InvalidOperationException("Could not deserialize to type");
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
