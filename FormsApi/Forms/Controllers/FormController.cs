using FormsApi.Contract;
using FormsApi.Contract.MetadataContainer;
using FormsApi.Contract.View;
using FormsApi.Forms.Services;
using FormsApi.Metadata.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Forms.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class FormController(IFormRegistry registry, IFormBuilderService formBuilder, IMetadataBuilderService metadataBuilder) : ControllerBase
{
    [HttpGet("{path}")]
    public ActionResult<FormResponse> GetForm([FromRoute] string path)
    {
        IForm? form = registry.TryGet(path);
        if (form is null)
            return NotFound();

        IReadOnlyList<View> views = form.ProvideBuilder(formBuilder);
        List<ModelMetadataContainer> metadatas = metadataBuilder.BuildMetadata(form.ModelType);

        return Ok(new FormResponse
        {
            ModelType = new TypeDto(form.ModelType),
            ModelMetadatas = metadatas,
            Views = views
        });
    }
}
