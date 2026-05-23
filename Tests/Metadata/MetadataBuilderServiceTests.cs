using FormsApi.Contract;
using FormsApi.Contract.MetadataContainer;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata;
using FormsApi.Metadata.Services;

namespace Tests.Metadata;

[TestFixture]
public class MetadataBuilderServiceTests
{
    private MetadataBuilderService _service = null!;
    private MetadataProcessors _processors = null!;

    [SetUp]
    public void SetUp()
    {
        _processors = new MetadataProcessors();
        _service = new MetadataBuilderService(_processors);
    }

    public class TestModel
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public List<SubModel> Children { get; set; } = [];
        public SubModel Address { get; set; } = null!;
    }

    public class SubModel
    {
        public string? City { get; set; }
    }

    public class TestModelMetadata : Metadata<TestModel>
    {
        public TestModelMetadata()
        {
            Text(x => x.Name).WithLabel("Test Name");
            Numeric(x => x.Age).WithMinValue(0);
        }
    }

    [Test]
    public void BuildMetadataDictionary_FindsMetadataDefinitions()
    {
        _service.CollectMetadataDictionary();

        List<ModelMetadataContainer> result = _service.BuildMetadata(typeof(TestModel));

        Assert.That(result, Is.Not.Null);
        ModelMetadataContainer modelMetadata = result.First(m => m.Type.GetRuntimeType() == typeof(TestModel));

        Assert.That(modelMetadata.PropertyMetadatas, Has.Count.EqualTo(4));

        // If it found the metadata definition, it should have the label "Test Name"
        var nameProperty = modelMetadata.PropertyMetadatas[nameof(TestModel.Name)] as PrimitivePropertyMetadataContainer;
        Assert.That(nameProperty, Is.Not.Null);
        Assert.That(nameProperty!.Metadatas!.Any(m => m is LabelMetadata l && l.Value is Constant c && c.Value.ToString() == "Test Name"), Is.True);
    }

    [Test]
    public void BuildMetadata_Recursive_BuildsNestedMetadata()
    {
        _service.CollectMetadataDictionary();
        List<ModelMetadataContainer> result = _service.BuildMetadata(typeof(TestModel));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Any(m => m.Type.GetRuntimeType() == typeof(TestModel)), Is.True);
            Assert.That(result.Any(m => m.Type.GetRuntimeType() == typeof(SubModel)), Is.True);
        }

    }

    [Test]
    public void BuildMetadata_PrimitiveType_UsesDefaultInputTypeWhenNoMetadataDefinition()
    {
        _service.CollectMetadataDictionary();
        List<ModelMetadataContainer> result = _service.BuildMetadata(typeof(SubModel));

        var cityProperty = result.First().PropertyMetadatas[nameof(SubModel.City)] as PrimitivePropertyMetadataContainer;
        Assert.That(cityProperty, Is.Not.Null);
        Assert.That(cityProperty!.Metadatas!.Any(m => m is ControlTypeMetadata i && i.Value == ControlType.Text), Is.True);
    }

    [Test]
    public void BuildMetadata_Enumerable_BuildsCorrectMetadata()
    {
        _service.CollectMetadataDictionary();
        List<ModelMetadataContainer> result = _service.BuildMetadata(typeof(TestModel));

        ModelMetadataContainer modelMetadata = result.First(m => m.Type.GetRuntimeType() == typeof(TestModel));
        PropertyMetadataContainer childrenProperty = modelMetadata.PropertyMetadatas[nameof(TestModel.Children)];

        using (Assert.EnterMultipleScope())
        {
            Assert.That(childrenProperty, Is.InstanceOf<ArrayMetadataContainer>());
            Assert.That(((ArrayMetadataContainer)childrenProperty).EnumeratedType.GetRuntimeType(), Is.EqualTo(typeof(SubModel)));
        }

    }
}
