using FormsApi.Builder;
using FormsApi.Builder.Field;
using FormsApi.Builder.View;
using FormsApi.Common.Types;
using FormsApi.Definition;
using FormsApi.Definition.Field;
using FormsApi.Definition.Metadata;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;
using FormsApi.Recalculate;
using NUnit.Framework;

namespace Tests.Builder.FormBuilder;

public class RecalculateEventBuilderTests
{
    private FormDefinition _form = new TestForm().Build();

    [Test]
    public void MethodWithModelParameter_ShouldSendAll()
    {
        RecalculateEvent? recalculate = (_form.View as FieldViewDefinition)?.Fields.ToList()[0]
            .FieldMetadatas?.SingleOrDefault(x => x.Type == MetadataType.RecalculateEvent)?.Value as RecalculateEvent;
        Assert.That(recalculate, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(recalculate.Service, Is.EqualTo(new SerializedType(typeof(TestService))));
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

    private class TestForm : FormBuilder<TestModel>
    {
        protected override ViewBuilder<TestModel> View => new FieldViewBuilder<TestModel>()
        {
            { m => m.B, p => p.AddRecalc<TestService>(s => s.RecalculateMethod )},
        };
    }
}
