using FormsApi.Common.Registry;
using FormsApi.Form;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;
using FormsApi.Form.View;

namespace Tests.Builder.FormBuilder;

public class FieldAugmentTests
{
    private readonly FormModel _form = new TestFormBuilder().Build(new RepositoryTypeRegistry());

    [Test]
    public void FieldAugments_AreAppliedCorrectly()
    {
        List<BaseField> fields = ((CombinedView)_form.View).Views
            .Select(x => x as DataView).Last(x => x != null)?.Fields.ToList()!;

        AssertAugmentHasValue(fields, nameof(TestModel.BoolProperty), nameof(CheckBoxInput.Width), new PercentSize(50));
        AssertAugmentHasValue(fields, nameof(TestModel.CurrencyProperty), nameof(CurrencyInput.Disabled), new Constant(true));
        AssertAugmentHasValue(fields, nameof(TestModel.DateProperty), nameof(DateInput.MaxValue), new Constant(new DateOnly(2025, 01, 01)));
        AssertAugmentHasValue(fields, nameof(TestModel.DecimalProperty), nameof(NumericInput.Precision), new Constant(4));
        AssertAugmentHasValue(fields, nameof(TestModel.IntProperty), nameof(NumericInput.MinValue), new Property(nameof(TestModel.MinValueProperty)));
        AssertAugmentHasValue(fields, nameof(TestModel.StringListProperty), nameof(TextAreaInput.Hidden), new Constant(true));
        AssertAugmentHasValue(fields, nameof(TestModel.StringProperty), nameof(TextInput.Label), new Constant("Test Label"));

        TimeInput timeField = fields.OfType<TimeInput>()
                                   .First(f => f.Property.Equals(nameof(TestModel.TimeProperty)));

        Assert.That(timeField.PropertiesToUpdateOnChange, Is.EquivalentTo(new List<string>() { nameof(TestModel.BoolProperty) }));
    }

    private void AssertAugmentHasValue(List<BaseField> fields, string inputName, string augmentName, object value)
    {
        Assert.That(fields, Has.One.With
            .Property(nameof(BaseInput.Property)).EqualTo(inputName)
            .And.With.Property(augmentName).EqualTo(value));
    }

}
