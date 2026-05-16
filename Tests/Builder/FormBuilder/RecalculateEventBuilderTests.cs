using FormsApi.Builder;
using FormsApi.Common.Types;
using FormsApi.Definition;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;
using FormsApi.Recalculate;

namespace Tests.Builder.FormBuilder;

public class RecalculateEventBuilderTests
{
    private FormDto _form = new TestForm().Build();

    [Test]
    public void MethodWithModelParameter_ShouldSendAll()
    {
        RecalculateEventDto? recalculate = (_form.View as FieldViewDto)?.Fields.ToList()[0]
            .FieldMetadatas?.SingleOrDefault(x => x.Type == MetadataType.RecalculateEvent)?.Value as RecalculateEventDto;
        Assert.That(recalculate, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(recalculate.Service, Is.EqualTo(new TypeDto(typeof(TestService))));
            Assert.That(recalculate.Method, Is.EqualTo(nameof(TestService.RecalculateMethod)));
        }
    }

    private class TestModel
    {
        public Button B { get; set; }
    }

    private class TestService
    {
        public PostRecalculateEvent? RecalculateMethod(TestModel model) => null;

    }

    private class TestForm : Form<TestModel>
    {
        protected override ViewBuilder<TestModel> View => new FieldViewBuilder<TestModel>()
        {
            { m => m.B, p => p.AddRecalc<TestService>(s => s.RecalculateMethod )},
        };
    }
}
