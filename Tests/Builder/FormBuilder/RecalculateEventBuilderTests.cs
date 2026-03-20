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
            Assert.That(recalculate.PropertiesToSend, Is.InstanceOf<SendNone>());
        }
    }

    [Test]
    public void MethodWithInterfaceParameter_ShouldSendSome()
    {
        string[] expectedProps = ["Prop1", "Prop2"];
        RecalculateEvent? recalculate = ((_form.View as DataView)?.Fields.ToList()[1] as BaseInput)?.RecalculateEvent;
        Assert.That(recalculate, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(recalculate.Service, Is.EqualTo(new SerializedType(typeof(TestService))));
            Assert.That(recalculate.Method, Is.EqualTo(nameof(TestService.ShouldSendSome)));
            Assert.That(recalculate.PropertiesToSend, Is.InstanceOf<SendSome>()
                .With.Property(nameof(SendSome.Names)).EquivalentTo(expectedProps));
        }
    }

    [Test]
    public void MethodWithModelParameter_ShouldSendAll()
    {
        RecalculateEvent? recalculate = ((_form.View as DataView)?.Fields.ToList()[2] as BaseInput)?.RecalculateEvent;
        Assert.That(recalculate, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(recalculate.Service, Is.EqualTo(new SerializedType(typeof(TestService))));
            Assert.That(recalculate.Method, Is.EqualTo(nameof(TestService.ShouldSendAll)));
            Assert.That(recalculate.PropertiesToSend, Is.InstanceOf<SendAll>());
        }
    }

    [Test]
    public void InvalidModel_ShouldThrow()
    {
        var e = Assert.Throws<InvalidOperationException>(() => (new InvalidTestForm().Build().View as DataView)?.Fields.ToList());
        Assert.That(e.Message, Does.Contain("must be assignable to method parameter/return type"));
    }

    private interface IIntPropertyRecalculable
    {
        int Prop1 { get; set; }
        int Prop2 { get; set; }
        int Prop3 { get; }
    }
    private class TestModel : IIntPropertyRecalculable
    {
        public int Prop1 { get; set; }
        public int Prop2 { get; set; }

        public int Prop3 { get; }
        public string? Prop4 { get; set; }
    }

    private class TestService
    {
        public RecalculateEventResult<TestModel> ShouldSendNone() => null!;
        public RecalculateEventResult<IIntPropertyRecalculable> ShouldSendSome(IIntPropertyRecalculable model) => null!;
        public RecalculateEventResult<TestModel> ShouldSendAll(TestModel model) => null!;
        public RecalculateEventResult<string> ShouldBeInvalid(string param) => null!;

    }

    private class TestForm : FormBuilder<TestModel>
    {
        protected override ViewBuilder<TestModel> View => new DataViewBuilder<TestModel>()
        {
            { m => Button.OnModel(m).WithRecalculate<TestService>(s => s.ShouldSendNone) },
            { m => m.Prop1, p => p.WithRecalculate<TestService, IIntPropertyRecalculable>(s => s.ShouldSendSome) },
            { m => m.Prop4, p => p.WithRecalculate<TestService>(s => s.ShouldSendAll )},
        };
    }

    private class InvalidTestForm : FormBuilder<TestModel>
    {
        protected override ViewBuilder<TestModel> View => new DataViewBuilder<TestModel>()
        {
            { m => Button.OnModel(m).WithRecalculate<TestService, string>(s => s.ShouldBeInvalid) },
        };
    }
}
