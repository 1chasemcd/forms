using FormsApi.Definition;
using FormsApi.Definition.Field;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace Tests.Builder.FormBuilder;

public class DataViewTests
{
    private readonly FormDefinition _form = new TestFormBuilder().Build();

    [TestCase(nameof(TestModel.BoolProperty), 0)]
    [TestCase(nameof(TestModel.CurrencyProperty), 1)]
    [TestCase(nameof(TestModel.DateProperty), 2)]
    [TestCase(nameof(TestModel.DecimalProperty), 3)]
    [TestCase(nameof(TestModel.IntProperty), 4)]
    [TestCase(nameof(TestModel.TextAreaProperty), 5)]
    [TestCase(nameof(TestModel.StringProperty), 6)]
    [TestCase(nameof(TestModel.TimeProperty), 7)]
    public void DataView_MaintainsCorrectFieldOrder(string propertyName, int expectedIndex)
    {
        var fields = ((CombinedViewDefinition)_form.View).Views
                    .Select(x => x as FieldViewDefinition).Where(x => x != null).ToList()[0]!.Fields.ToList();

        Assert.That(fields, Is.Not.Null);
        Assert.That(fields, Has.ItemAt(expectedIndex)
            .With.Property(nameof(FieldDefinition.Property)).EqualTo(propertyName));
    }

    [TestCase(nameof(TestModel.BoolProperty), FieldType.CheckBox)]
    [TestCase(nameof(TestModel.CurrencyProperty), FieldType.Currency)]
    [TestCase(nameof(TestModel.DateProperty), FieldType.Date)]
    [TestCase(nameof(TestModel.DecimalProperty), FieldType.Numeric)]
    [TestCase(nameof(TestModel.IntProperty), FieldType.Numeric)]
    [TestCase(nameof(TestModel.TextAreaProperty), FieldType.TextArea)]
    [TestCase(nameof(TestModel.StringProperty), FieldType.Text)]
    [TestCase(nameof(TestModel.TimeProperty), FieldType.Time)]
    [TestCase(nameof(TestModel.Button), FieldType.Button)]
    [TestCase(nameof(TestModel.StaticText), FieldType.LabelValue)]
    public void DataView_MapsInputFieldTypesCorrectly(string inputName, FieldType expectedInputType)
    {
        List<FieldDefinition> fields = ((CombinedViewDefinition)_form.View).Views
            .Select(x => x as FieldViewDefinition).Where(x => x != null).ToList()[0]?.Fields.ToList()!;

        Assert.That(fields, Has.One.With.Property(nameof(FieldDefinition.Type)).EqualTo(expectedInputType)
            .And.With.Property(nameof(FieldDefinition.Property)).EqualTo(inputName));
    }
}
