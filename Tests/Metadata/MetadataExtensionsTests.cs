using FormsApi.Common;
using FormsApi.Metadata;
using Moq;

namespace Tests.Metadata;

[TestFixture]
public class MetadataExtensionsTests
{
    public sealed class TestModel
    {
        public string? Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVisible { get; set; }
        public int MaxLength { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }

    private MockMetadataBuilder<TestModel> _builder = null!;

    [SetUp]
    public void SetUp()
    {
        _builder = new MockMetadataBuilder<TestModel>();
    }

    [Test]
    public void Disabled_SetsEnabledToFalse()
    {
        MockMetadataBuilder<TestModel> result = _builder.Disabled();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Enabled.InnerValue(), Is.False);
        });
    }

    [Test]
    public void EnabledWhen_SetsEnabledExpression()
    {
        MockMetadataBuilder<TestModel> result = _builder.EnabledWhen(x => x.IsEnabled);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Enabled.InnerValue(), Is.EqualTo(nameof(TestModel.IsEnabled)));
        });
    }

    [Test]
    public void WithLabel_String_SetsLabel()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithLabel("Test Label");

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Label.InnerValue(), Is.EqualTo("Test Label"));
        });
    }

    [Test]
    public void WithLabel_Expression_SetsLabelExpression()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithLabel(x => x.Name);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Label.InnerValue(), Is.EqualTo(nameof(TestModel.Name)));
        });
    }

    [Test]
    public void WithMaxLength_Int_SetsMaxLength()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithMaxLength(50);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.MaxLength.InnerValue(), Is.EqualTo(50));
        });
    }

    [Test]
    public void WithMaxLength_Expression_SetsMaxLengthExpression()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithMaxLength(x => x.MaxLength);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.MaxLength.InnerValue(), Is.EqualTo(nameof(TestModel.MaxLength)));
        });
    }

    [Test]
    public void WithPrecision_Int_SetsPrecision()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithPrecision(5);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Precision.InnerValue(), Is.EqualTo(5));
        });
    }

    [Test]
    public void WithPrecision_Expression_SetsPrecisionExpression()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithPrecision(x => x.Precision);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Precision.InnerValue(), Is.EqualTo(nameof(TestModel.Precision)));
        });
    }

    [Test]
    public void WithScale_Int_SetsScale()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithScale(2);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Scale.InnerValue(), Is.EqualTo(2));
        });
    }

    [Test]
    public void WithScale_Expression_SetsScaleExpression()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithScale(x => x.Scale);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Scale.InnerValue(), Is.EqualTo(nameof(TestModel.Scale)));
        });
    }

    [Test]
    public void OnChange_SetsFormServiceMethod()
    {
        Mock<IFormServiceMethod<TestModel>> recalc = new();

        MockMetadataBuilder<TestModel> result = _builder.OnChange(recalc.Object);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.FormServiceMethod, Is.SameAs(recalc.Object));
        });
    }

    [Test]
    public void Required_SetsRequiredToTrue()
    {
        MockMetadataBuilder<TestModel> result = _builder.Required();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Required.InnerValue(), Is.True);
        });
    }

    [Test]
    public void RequiredWhen_SetsRequiredExpression()
    {
        MockMetadataBuilder<TestModel> result = _builder.RequiredWhen(x => x.IsRequired);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Required.InnerValue(), Is.EqualTo(nameof(TestModel.IsRequired)));
        });
    }

    [Test]
    public void WithMinValue_Value_SetsMinValue()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithMinValue(10);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.MinValue.InnerValue(), Is.EqualTo(10));
        });
    }

    [Test]
    public void WithMinValue_Expression_SetsMinValueExpression()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithMinValue(x => x.MinValue);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.MinValue.InnerValue(), Is.EqualTo(nameof(TestModel.MinValue)));
        });
    }

    [Test]
    public void WithMaxValue_Value_SetsMaxValue()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithMaxValue(100);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.MaxValue.InnerValue(), Is.EqualTo(100));
        });
    }

    [Test]
    public void WithMaxValue_Expression_SetsMaxValueExpression()
    {
        MockMetadataBuilder<TestModel> result = _builder.WithMaxValue(x => x.MaxValue);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.MaxValue.InnerValue(), Is.EqualTo(nameof(TestModel.MaxValue)));
        });
    }

    [Test]
    public void Hidden_SetsVisibleToFalse()
    {
        MockMetadataBuilder<TestModel> result = _builder.Hidden();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Visible.InnerValue(), Is.False);
        });
    }

    [Test]
    public void VisibleWhen_SetsVisibleExpression()
    {
        MockMetadataBuilder<TestModel> result = _builder.VisibleWhen(x => x.IsVisible);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.SameAs(_builder));
            Assert.That(_builder.Visible.InnerValue(), Is.EqualTo(nameof(TestModel.IsVisible)));
        });
    }
}
