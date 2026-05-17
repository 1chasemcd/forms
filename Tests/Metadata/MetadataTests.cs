using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata;
using FormsApi.Metadata.Builders;

namespace Tests.Metadata;

[TestFixture]
public class MetadataTests : Metadata<MetadataTests.TestModel>
{
    public sealed class TestModel
    {
        public object? ObjectProperty { get; set; }
        public bool BoolProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public string? StringProperty { get; set; }
        public DateOnly DateProperty { get; set; }
        public TimeOnly TimeProperty { get; set; }
    }

    private void AssertControlWasAdded(IMetadataBuilder<TestModel> created, InputType expectedInputType, string expectedPropertyName)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(created.GetInputType(), Is.EqualTo(expectedInputType));
            Assert.That(MetadataBuilders.TryGetValue(expectedPropertyName, out IMetadataBuilder<TestModel>? value), Is.True);
            Assert.That(value, Is.EqualTo(created));
        }
    }

    [SetUp]
    public void SetUp()
    {
        MetadataBuilders.Clear();
    }

    [Test]
    public void Button_AddsToMetadataDictionary()
    {
        ButtonInputMetadataBuilder<TestModel> result = Button(x => x.ObjectProperty);
        AssertControlWasAdded(result, InputType.Button, nameof(TestModel.ObjectProperty));
    }

    [Test]
    public void CheckBox_AddsToMetadataDictionary()
    {
        CheckBoxInputMetadataBuilder<TestModel> result = CheckBox(x => x.BoolProperty);
        AssertControlWasAdded(result, InputType.CheckBox, nameof(TestModel.BoolProperty));
    }

    [Test]
    public void Currency_AddsToMetadataDictionary()
    {
        CurrencyInputMetadataBuilder<TestModel> result = Currency(x => x.DecimalProperty);
        AssertControlWasAdded(result, InputType.Currency, nameof(TestModel.DecimalProperty));
    }

    [Test]
    public void Date_AddsToMetadataDictionary()
    {
        DateInputMetadataBuilder<TestModel> result = Date(x => x.DateProperty);
        AssertControlWasAdded(result, InputType.Date, nameof(TestModel.DateProperty));
    }

    [Test]
    public void LabelValue_AddsToMetadataDictionary()
    {
        LabelValueMetadataBuilder<TestModel> result = LabelValue(x => x.StringProperty);
        AssertControlWasAdded(result, InputType.LabelValue, nameof(TestModel.StringProperty));
    }

    [Test]
    public void Numeric_AddsToMetadataDictionary()
    {
        NumericInputMetadataBuilder<TestModel, decimal> result = Numeric(x => x.DecimalProperty);
        AssertControlWasAdded(result, InputType.Numeric, nameof(TestModel.DecimalProperty));
    }

    [Test]
    public void TextArea_AddsToMetadataDictionary()
    {
        TextAreaInputMetadataBuilder<TestModel> result = TextArea(x => x.StringProperty);
        AssertControlWasAdded(result, InputType.TextArea, nameof(TestModel.StringProperty));
    }

    [Test]
    public void Text_AddsToMetadataDictionary()
    {
        TextInputMetadataBuilder<TestModel> result = Text(x => x.StringProperty);
        AssertControlWasAdded(result, InputType.Text, nameof(TestModel.StringProperty));
    }

    [Test]
    public void Time_AddsToMetadataDictionary()
    {
        TimeInputMetadataBuilder<TestModel> result = Time(x => x.TimeProperty);
        AssertControlWasAdded(result, InputType.Time, nameof(TestModel.TimeProperty));
    }

    [Test]
    public void Button_CalledMultipleTimes_ReturnsSameValue()
    {
        ButtonInputMetadataBuilder<TestModel> result1 = Button(x => x.ObjectProperty);
        ButtonInputMetadataBuilder<TestModel> result2 = Button(x => x.ObjectProperty);
        Assert.That(result1, Is.SameAs(result2));
    }

    [Test]
    public void Button_CalledAfterTextForSameProperty_Throws()
    {
        Text(x => x.StringProperty);
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(() => Button(x => x.StringProperty));
        Assert.That(ex?.Message, Does.Contain("was already assigned a different input type"));
    }
}
