using FormsApi.Contract;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Contract.MetadataCollection;
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

        List<ModelMetadataCollectionDto> result = _service.BuildMetadata(typeof(TestModel));

        Assert.That(result, Is.Not.Null);
        ModelMetadataCollectionDto modelMetadata = result.First(m => m.Type.GetRuntimeType() == typeof(TestModel));

        Assert.That(modelMetadata.PropertyMetadatas, Has.Count.EqualTo(4));

        // If it found the metadata definition, it should have the label "Test Name"
        var nameProperty = modelMetadata.PropertyMetadatas[nameof(TestModel.Name)] as PrimitivePropertyMetadataDto;
        Assert.That(nameProperty, Is.Not.Null);
        Assert.That(nameProperty!.Metadatas!.Any(m => m is LabelMetadataDto l && l.Value is ConstantDto c && c.Value.ToString() == "Test Name"), Is.True);
    }

    [Test]
    public void BuildMetadata_Recursive_BuildsNestedMetadata()
    {
        _service.CollectMetadataDictionary();
        List<ModelMetadataCollectionDto> result = _service.BuildMetadata(typeof(TestModel));

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
        List<ModelMetadataCollectionDto> result = _service.BuildMetadata(typeof(SubModel));

        var cityProperty = result.First().PropertyMetadatas[nameof(SubModel.City)] as PrimitivePropertyMetadataDto;
        Assert.That(cityProperty, Is.Not.Null);
        Assert.That(cityProperty!.Metadatas!.Any(m => m is ControlTypeMetadataDto i && i.Value == ControlType.Text), Is.True);
    }

    [Test]
    public void BuildMetadata_Enumerable_BuildsCorrectMetadata()
    {
        _service.CollectMetadataDictionary();
        List<ModelMetadataCollectionDto> result = _service.BuildMetadata(typeof(TestModel));

        ModelMetadataCollectionDto modelMetadata = result.First(m => m.Type.GetRuntimeType() == typeof(TestModel));
        IPropertyMetadataDto childrenProperty = modelMetadata.PropertyMetadatas[nameof(TestModel.Children)];

        using (Assert.EnterMultipleScope())
        {
            Assert.That(childrenProperty, Is.InstanceOf<EnumerablePropertyMetadataDto>());
            Assert.That(((EnumerablePropertyMetadataDto)childrenProperty).EnumeratedType.GetRuntimeType(), Is.EqualTo(typeof(SubModel)));
        }

    }
}
