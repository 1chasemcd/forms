using FormsApi.Form;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;
using FormsApi.Form.View;

namespace Tests.Builder.FormBuilder;

public class DataViewTests
{
    private readonly FormDefinition _form = new TestFormBuilder().Build();

    [TestCase(nameof(TestModel.BoolProperty), 0)]
    [TestCase(nameof(TestModel.CurrencyProperty), 1)]
    [TestCase(nameof(TestModel.DateProperty), 2)]
    [TestCase(nameof(TestModel.DecimalProperty), 3)]
    [TestCase(nameof(TestModel.IntProperty), 4)]
    [TestCase(nameof(TestModel.StringListProperty), 5)]
    [TestCase(nameof(TestModel.StringProperty), 6)]
    [TestCase(nameof(TestModel.TimeProperty), 7)]
    public void DataView_MaintainsCorrectFieldOrder(string propertyName, int expectedIndex)
    {
        var fields = ((CombinedView)_form.View).Views
                    .Select(x => x as DataView).Where(x => x != null).ToList()[0]!.Fields.ToList();

        Assert.That(fields, Is.Not.Null);
        Assert.That(fields, Has.ItemAt(expectedIndex)
            .With.Property(nameof(BaseInput.Property)).EqualTo(propertyName));
    }

    [TestCase(nameof(TestModel.BoolProperty), typeof(CheckBoxInput))]
    [TestCase(nameof(TestModel.CurrencyProperty), typeof(CurrencyInput))]
    [TestCase(nameof(TestModel.DateProperty), typeof(DateInput))]
    [TestCase(nameof(TestModel.DecimalProperty), typeof(NumericInput))]
    [TestCase(nameof(TestModel.IntProperty), typeof(NumericInput))]
    [TestCase(nameof(TestModel.StringListProperty), typeof(TextAreaInput))]
    [TestCase(nameof(TestModel.StringProperty), typeof(TextInput))]
    [TestCase(nameof(TestModel.TimeProperty), typeof(TimeInput))]
    public void DataView_MapsInputFieldTypesCorrectly(string inputName, Type expectedInputType)
    {
        List<BaseField> fields = ((CombinedView)_form.View).Views
            .Select(x => x as DataView).Where(x => x != null).ToList()[0]?.Fields.ToList()!;

        Assert.That(fields, Has.One.With.InstanceOf(expectedInputType)
            .With.Property(nameof(BaseInput.Property)).EqualTo(inputName));
    }

    [Test]
    public void DataView_RendersButtonFieldCorrectly()
    {
        ButtonField? button = ((CombinedView)_form.View).Views
            .Select(x => x as DataView).First(x => x != null)?.Fields.SingleOrDefault(f => f is ButtonField) as ButtonField;


        Assert.That(button, Has.Property(nameof(ButtonField.OnChange))
            .With.Property(nameof(OnChangeEvent.FormAction))
            .With.Property(nameof(FormAction.Service))
            .EqualTo(new SerializedType(typeof(TestService))));

        Assert.That(button, Has.Property(nameof(ButtonField.OnChange))
            .With.Property(nameof(OnChangeEvent.FormAction))
            .With.Property(nameof(FormAction.Method))
            .EqualTo(nameof(TestService.PerformAction)));
    }
}
