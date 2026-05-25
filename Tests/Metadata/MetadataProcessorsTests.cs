using FormsApi.Common;
using FormsApi.Contract;
using FormsApi.Contract.PostRequest;
using FormsApi.Contract.PropertyMetadata;
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
        public PostRequestAction DoThing(TestModel _) => null!;
    }

    private T AssertProcessorApplied<T>(IMetadataBuilder<TestModel> subject)
        where T : PropertyMetadata
    {
        IEnumerable<Func<IMetadataBuilder<TestModel>, PropertyMetadata?>> processors =
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

        ControlTypeMetadata result = AssertProcessorApplied<ControlTypeMetadata>(builderMock.Object);
        Assert.That(result.Value, Is.EqualTo(ControlType.Text));
    }

    [Test]
    public void EnabledProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Enabled = true
        };

        EnabledMetadata result = AssertProcessorApplied<EnabledMetadata>(builder);
        Assert.That(result.Value.InnerValue(), Is.True);
    }

    [Test]
    public void LabelProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Label = "Test Label"
        };

        LabelMetadata result = AssertProcessorApplied<LabelMetadata>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo("Test Label"));
    }

    [Test]
    public void MaxLengthProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            MaxLength = 5
        };

        MaxLengthMetadata result = AssertProcessorApplied<MaxLengthMetadata>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo(5));
    }

    [Test]
    public void PrecisionProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Precision = 5
        };

        PrecisionMetadata result = AssertProcessorApplied<PrecisionMetadata>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo(5));
    }

    [Test]
    public void ScaleProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Scale = 5
        };

        ScaleMetadata result = AssertProcessorApplied<ScaleMetadata>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo(5));
    }

    [Test]
    public void ServiceMethodProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            ServiceMethod = new ServiceMethodBuilder<TestModel, TestModel>(x => x.DoThing)
        };

        ServiceMethodMetadata result = AssertProcessorApplied<ServiceMethodMetadata>(builder);
        Assert.That(result.Value,
            Has.Property(nameof(ServiceMethod.Service))
            .EqualTo(new TypeDto(typeof(TestModel)))
            .And.Property(nameof(ServiceMethod.Method))
            .EqualTo(nameof(TestModel.DoThing)));
    }

    [Test]
    public void RequiredProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Required = true
        };

        RequiredMetadata result = AssertProcessorApplied<RequiredMetadata>(builder);
        Assert.That(result.Value.InnerValue(), Is.True);
    }

    [Test]
    public void MaxValueProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            MaxValue = 50
        };

        MaxValueMetadata result = AssertProcessorApplied<MaxValueMetadata>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo(50));
    }

    [Test]
    public void MinValueProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            MinValue = 10
        };

        MinValueMetadata result = AssertProcessorApplied<MinValueMetadata>(builder);
        Assert.That(result.Value.InnerValue(), Is.EqualTo(10));
    }

    [Test]
    public void VisibleProcessor_ReturnsCorrectDto()
    {
        var builder = new MockMetadataBuilder<TestModel>
        {
            Visible = false
        };

        VisibleMetadata result = AssertProcessorApplied<VisibleMetadata>(builder);
        Assert.That(result.Value.InnerValue(), Is.False);
    }
}
