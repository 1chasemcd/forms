using FormsApi.Builder;
using FormsApi.Builder.Field;
using FormsApi.Builder.View;
using FormsApi.Form;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;
using FormsApi.Form.View;
using FormsApi.Recalculate;
using NUnit.Framework;

namespace Tests.Builder.FormBuilder;

public class RecalculateEventBuilderTests
{
    private FormDefinition _form = new TestForm().Build();
    [Test]
    public void MethodWithNoParameters_ShouldSendNone()
    {
        RecalculateEvent? recalculate = ((_form.View as DataView)?.Fields.ToList()[0] as ButtonField)?.RecalculateEvent;
        Assert.That(recalculate, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(recalculate.Service, Is.EqualTo(new SerializedType(typeof(TestService))));
            Assert.That(recalculate.Method, Is.EqualTo(nameof(TestService.ShouldSendNone)));
            Assert.That(recalculate.DontSendModel, Is.True);
        }
    }

    [Test]
    public void MethodWithModelParameter_ShouldSendAll()
    {
        RecalculateEvent? recalculate = ((_form.View as DataView)?.Fields.ToList()[1] as BaseInput)?.RecalculateEvent;
        Assert.That(recalculate, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(recalculate.Service, Is.EqualTo(new SerializedType(typeof(TestService))));
            Assert.That(recalculate.Method, Is.EqualTo(nameof(TestService.ShouldSendAll)));
            Assert.That(recalculate.DontSendModel, Is.False);
        }
    }

    private class TestModel
    {
        public int Prop1 { get; set; }
        public int Prop2 { get; set; }

        public int Prop3 { get; }
        public string? Prop4 { get; set; }
    }

    private class TestService
    {
        public PostRecalculateEvent? ShouldSendNone() => null;
        public PostRecalculateEvent? ShouldSendAll(TestModel model) => null;

    }

    private class TestForm : FormBuilder<TestModel>
    {
        protected override ViewBuilder<TestModel> View => new DataViewBuilder<TestModel>()
        {
            { m => Button.OnModel(m).WithRecalculate<TestService>(s => s.ShouldSendNone) },
            { m => m.Prop4, p => p.WithRecalculate<TestService>(s => s.ShouldSendAll )},
        };
    }
}
