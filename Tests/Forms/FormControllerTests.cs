using FormsApi.Contract;
using FormsApi.Contract.MetadataCollection;
using FormsApi.Contract.View;
using FormsApi.Forms.Controllers;
using FormsApi.Forms.Services;
using FormsApi.Metadata.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Forms;

[TestFixture]
public class FormControllerTests
{
    private sealed class TestModel
    {
        public string? Name { get; set; }
    }

    [Test]
    public void GetForm_WhenFormDoesNotExist_ReturnsNotFound()
    {
        var registry = new Mock<IFormRegistry>();
        var metadataService = new Mock<IMetadataBuilderService>();

        registry.Setup(x => x.TryGet("missing"))
            .Returns((Tuple<Type, BaseViewDto>?)null);

        var sut = new FormController(
            registry.Object,
            metadataService.Object);

        ActionResult<FormDto> result = sut.GetForm("missing");

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public void GetForm_WhenFormExists_ReturnsFormDto()
    {
        var registry = new Mock<IFormRegistry>();
        var metadataService = new Mock<IMetadataBuilderService>();

        BaseViewDto view = new Mock<BaseViewDto>().Object;

        var metadata = new List<ModelMetadataCollectionDto>
        {
            new()
            {
                Type = new TypeDto(typeof(TestModel)),
                PropertyMetadatas = []
            }
        };

        registry.Setup(x => x.TryGet("test"))
            .Returns(new Tuple<Type, BaseViewDto>(
                typeof(TestModel),
                view));

        metadataService.Setup(x => x.BuildMetadata(typeof(TestModel)))
            .Returns(metadata);

        var sut = new FormController(
            registry.Object,
            metadataService.Object);

        ActionResult<FormDto> result = sut.GetForm("test");

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var okResult = (OkObjectResult)result.Result!;

        Assert.That(okResult.Value, Is.TypeOf<FormDto>());

        var dto = (FormDto)okResult.Value!;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(dto.ModelType.GetRuntimeType(), Is.EqualTo(typeof(TestModel)));
            Assert.That(dto.View, Is.EqualTo(view));
            Assert.That(dto.ModelMetadatas, Is.EqualTo(metadata));
        }

    }
}
