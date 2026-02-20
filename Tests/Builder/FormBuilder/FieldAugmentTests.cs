using FormsApi.Form;
using FormsApi.Form.Field;
using FormsApi.Form.View;

namespace Tests.Builder.FormBuilder;

public class FieldAugmentTests
{
    private readonly FormModel _form = new TestFormBuilder().Build();

    [Test]
    public void DataView_FieldAugments()
    {
        List<BaseField> fields = ((CombinedView)_form.View).Views
            .Select(x => x as DataView).Last(x => x != null)?.Fields.ToList()!;

        AssertAugmentHasValue(nameof(TestModel.BoolProperty), nameof(CheckBoxInput.Width), new FormElementSize(50));
        AssertAugmentHasValue(nameof(TestModel.CurrencyProperty), nameof(CurrencyInput.Disabled), new Constant<bool>(true));
        AssertAugmentHasValue(nameof(TestModel.DateProperty), nameof(DateInput.MaxValue), new Constant<DateOnly>(new DateOnly(2025, 01, 01)));
        AssertAugmentHasValue(nameof(TestModel.DecimalProperty), nameof(NumericInput.Precision), new Constant<int>(4));
        AssertAugmentHasValue(nameof(TestModel.IntProperty), nameof(NumericInput.MinValue), new Property<int>(nameof(TestModel.MinValueProperty)));
        AssertAugmentHasValue(nameof(TestModel.StringListProperty), nameof(TextAreaInput.Hidden), new Constant<bool>(true));
        AssertAugmentHasValue(nameof(TestModel.StringProperty), nameof(TextInput.Label), new Constant<string>("Test Label"));

        Assert.That(fields, Has.One.With
            .Property(nameof(BaseInput.Property)).EqualTo(nameof(TestModel.TimeProperty))
            .And.With.Property(nameof(TimeInput.PropertiesToUpdateOnChange)).EquivalentTo(new List<string>() { nameof(TestModel.BoolProperty) }));

        void AssertAugmentHasValue(string inputName, string augmentName, object value) => Assert.That(fields, Has.One.With
            .Property(nameof(BaseInput.Property)).EqualTo(inputName)
            .And.With.Property(augmentName).EqualTo(value));
    }
}
