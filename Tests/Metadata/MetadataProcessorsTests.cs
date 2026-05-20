using FormsApi.Common;
using FormsApi.Contract;
using FormsApi.Contract.ControlMetadata;
using FormsApi.FormService;
using FormsApi.Metadata.Builders;
using FormsApi.Metadata.Services;
using Moq;

namespace Tests.Metadata;

[TestFixture]
public class MetadataProcessorsTests
{
    private MetadataProcessors _processors = null!;

    [SetUp]
    public void SetUp()
    {
        _processors = new MetadataProcessors();
    }

    public class TestModel
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public FormServicePostAction DoThing(TestModel _) => null!;
    }

    private T AssertProcessorApplied<T>(IMetadataBuilder<TestModel> subject)
        where T : BaseControlMetadataDto
    {
        IEnumerable<Func<IMetadataBuilder<TestModel>, BaseControlMetadataDto?>> processors =
            _processors.GetProcessors<TestModel>();

        var all = processors.Select(p => p.Invoke(subject)).Where(x => x is not null && x is T).Cast<T>().ToList();
        if (all.Count != 1) Assert.Fail($"Expected exactly one processor to be applied but was {all.Count}");
        return all.Single();
    }

    [Test]
    public void InputTypeProcessor_ReturnsCorrectDto()
    {
        var builderMock = new Mock<IMetadataBuilder<TestModel>>();
        builderMock.Setup(x => x.GetControlType()).Returns(ControlType.Text);

        ControlTypeMetadataDto result = AssertProcessorApplied<ControlTypeMetadataDto>(builderMock.Object);
        Assert.That(result.Value, Is.EqualTo(ControlType.Text));
    }

    [Test]
    public void EnabledProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Enabled = true
        };

        EnabledMetadataDto result = AssertProcessorApplied<EnabledMetadataDto>(builder);
        Assert.That(result.Value.InnerValue(), Is.True);
    }

    [Test]
    public void LabelProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Label = "Test Label"
        };

        LabelMetadataDto result = AssertProcessorApplied<LabelMetadataDto>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo("Test Label"));
    }

    [Test]
    public void MaxLengthProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            MaxLength = 5
        };

        MaxLengthMetadataDto result = AssertProcessorApplied<MaxLengthMetadataDto>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo(5));
    }

    [Test]
    public void PrecisionProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Precision = 5
        };

        PrecisionMetadataDto result = AssertProcessorApplied<PrecisionMetadataDto>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo(5));
    }

    [Test]
    public void ScaleProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Scale = 5
        };

        ScaleMetadataDto result = AssertProcessorApplied<ScaleMetadataDto>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo(5));
    }

    [Test]
    public void RecalculateProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            FormServiceMethod = new FormServiceMethod<TestModel, TestModel>(x => x.DoThing)
        };

        FormServiceMethodMetadataDto result = AssertProcessorApplied<FormServiceMethodMetadataDto>(builder);
        Assert.That(result.Value,
            Has.Property(nameof(FormServiceMethodDto.Service))
            .EqualTo(new TypeDto(typeof(TestModel)))
            .And.Property(nameof(FormServiceMethodDto.Method))
            .EqualTo(nameof(TestModel.DoThing)));
    }

    [Test]
    public void RequiredProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Required = true
        };

        RequiredMetadataDto result = AssertProcessorApplied<RequiredMetadataDto>(builder);
        Assert.That(result.Value.InnerValue(), Is.True);
    }

    [Test]
    public void MaxValueProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            MaxValue = 50
        };

        MaxValueMetadataDto result = AssertProcessorApplied<MaxValueMetadataDto>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo(50));
    }

    [Test]
    public void MinValueProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            MinValue = 10
        };

        MinValueMetadataDto result = AssertProcessorApplied<MinValueMetadataDto>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo(10));
    }

    [Test]
    public void VisibleProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Visible = false
        };

        VisibleMetadataDto result = AssertProcessorApplied<VisibleMetadataDto>(builder);
        Assert.That(result.Value.InnerValue(), Is.False);
    }
}
