using FormsApi.Contract.PropertyMetadata;
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

    private void AssertControlWasAdded(IMetadataBuilder<TestModel> created, ControlType expectedInputType, string expectedPropertyName)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(created.GetControlType(), Is.EqualTo(expectedInputType));
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
        ButtonMetadataBuilder<TestModel> result = Button(x => x.ObjectProperty);
        AssertControlWasAdded(result, ControlType.Button, nameof(TestModel.ObjectProperty));
    }

    [Test]
    public void CheckBox_AddsToMetadataDictionary()
    {
        CheckBoxMetadataBuilder<TestModel> result = CheckBox(x => x.BoolProperty);
        AssertControlWasAdded(result, ControlType.CheckBox, nameof(TestModel.BoolProperty));
    }

    [Test]
    public void Currency_AddsToMetadataDictionary()
    {
        CurrencyMetadataBuilder<TestModel> result = Currency(x => x.DecimalProperty);
        AssertControlWasAdded(result, ControlType.Currency, nameof(TestModel.DecimalProperty));
    }

    [Test]
    public void Date_AddsToMetadataDictionary()
    {
        DateMetadataBuilder<TestModel> result = Date(x => x.DateProperty);
        AssertControlWasAdded(result, ControlType.Date, nameof(TestModel.DateProperty));
    }

    [Test]
    public void LabelValue_AddsToMetadataDictionary()
    {
        LabelValueMetadataBuilder<TestModel> result = LabelValue(x => x.StringProperty);
        AssertControlWasAdded(result, ControlType.LabelValue, nameof(TestModel.StringProperty));
    }

    [Test]
    public void Numeric_AddsToMetadataDictionary()
    {
        NumericMetadataBuilder<TestModel, decimal> result = Numeric(x => x.DecimalProperty);
        AssertControlWasAdded(result, ControlType.Numeric, nameof(TestModel.DecimalProperty));
    }

    [Test]
    public void TextArea_AddsToMetadataDictionary()
    {
        TextAreaMetadataBuilder<TestModel> result = TextArea(x => x.StringProperty);
        AssertControlWasAdded(result, ControlType.TextArea, nameof(TestModel.StringProperty));
    }

    [Test]
    public void Text_AddsToMetadataDictionary()
    {
        TextMetadataBuilder<TestModel> result = Text(x => x.StringProperty);
        AssertControlWasAdded(result, ControlType.Text, nameof(TestModel.StringProperty));
    }

    [Test]
    public void Time_AddsToMetadataDictionary()
    {
        TimeMetadataBuilder<TestModel> result = Time(x => x.TimeProperty);
        AssertControlWasAdded(result, ControlType.Time, nameof(TestModel.TimeProperty));
    }

    [Test]
    public void Button_CalledMultipleTimes_ReturnsSameValue()
    {
        ButtonMetadataBuilder<TestModel> result1 = Button(x => x.ObjectProperty);
        ButtonMetadataBuilder<TestModel> result2 = Button(x => x.ObjectProperty);
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
