using FormsApi.Contract;
using FormsApi.Contract.MetadataContainer;
using FormsApi.Contract.View;
using FormsApi.Forms;
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

    private sealed class TestForm : Form<TestModel>
    {
        protected internal override IViewBuilder<TestModel> View => null!;
    }

    [Test]
    public void GetForm_WhenFormDoesNotExist_ReturnsNotFound()
    {
        var registry = new Mock<IFormRegistry>();
        var formService = new Mock<IFormBuilderService>();
        var metadataService = new Mock<IMetadataBuilderService>();

        registry.Setup(x => x.TryGet("missing"))
            .Returns((IForm?)null);

        var sut = new FormController(
            registry.Object,
            formService.Object,
            metadataService.Object);

        ActionResult<FormResponse> result = sut.GetForm("missing");

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public void GetForm_WhenFormExists_ReturnsFormDto()
    {
        var registry = new Mock<IFormRegistry>();
        var formService = new Mock<IFormBuilderService>();
        var metadataService = new Mock<IMetadataBuilderService>();

        var testForm = new TestForm();

        View view = new Mock<View>().Object;

        var metadata = new List<ModelMetadataContainer>
        {
            new()
            {
                Type = new TypeDto(typeof(TestModel)),
                PropertyMetadatas = []
            }
        };

        registry.Setup(x => x.TryGet("test"))
            .Returns(testForm);

        metadataService.Setup(x => x.BuildMetadata(typeof(TestModel)))
            .Returns(metadata);

        formService.Setup(x => x.BuildFormIntoViews(It.IsAny<TestForm>()))
            .Returns(new[] { view });

        var sut = new FormController(
            registry.Object,
            formService.Object,
            metadataService.Object);

        ActionResult<FormResponse> result = sut.GetForm("test");

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var okResult = (OkObjectResult)result.Result!;

        Assert.That(okResult.Value, Is.TypeOf<FormResponse>());

        var dto = (FormResponse)okResult.Value!;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(dto.ModelType.GetRuntimeType(), Is.EqualTo(typeof(TestModel)));
            Assert.That(dto.Views, Has.Count.EqualTo(1));
            Assert.That(dto.Views[0], Is.EqualTo(view));
            Assert.That(dto.ModelMetadatas, Is.EqualTo(metadata));
        }

    }
}
