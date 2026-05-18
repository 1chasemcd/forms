using FormsApi.Contract;
using FormsApi.Contract.MetadataCollection;
using FormsApi.Contract.View;
using FormsApi.Forms.Services;
using FormsApi.Metadata.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Forms.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class FormController(IFormRegistry registry, IMetadataBuilderService metadataService) : ControllerBase
{
    [HttpGet("{path}")]
    public ActionResult<FormDto> GetForm([FromRoute] string path)
    {
        Tuple<Type, BaseViewDto>? formInfo = registry.TryGet(path);
        if (formInfo is null)
            return NotFound();

        List<ModelMetadataCollectionDto> metadatas = metadataService.BuildMetadata(formInfo.Item1);

        return Ok(new FormDto
        {
            ModelType = new TypeDto(formInfo.Item1),
            View = formInfo.Item2,
            ModelMetadatas = metadatas
        });
    }
}
