using FormsApi.Common;
using FormsApi.Contract;

namespace Tests.Common;

[TestFixture]
public class PropertyOrConstantTests
{
    private sealed class TestModel
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    [Test]
    public void Build_WhenConstructedWithPropertySelector_ReturnsPropertyDto()
    {
        var sut = new PropertyOrConstant<TestModel, string>(x => x.Name);
        PropertyOrConstantDto result = sut.Build();
        Assert.That(result, Is.TypeOf<PropertyDto>());
        Assert.That(result.InnerValue(), Is.EqualTo(nameof(TestModel.Name)));
    }

    [Test]
    public void Build_WhenConstructedWithConstantValue_ReturnsConstantDto()
    {
        var sut = new PropertyOrConstant<TestModel, string>("test");
        PropertyOrConstantDto result = sut.Build();
        Assert.That(result, Is.TypeOf<ConstantDto>());
        Assert.That(result.InnerValue(), Is.EqualTo("test"));
    }

    [Test]
    public void Build_WhenConstructedUsingImplicitConstantConversion_ReturnsConstantDto()
    {
        PropertyOrConstant<TestModel, int> sut = 42;
        PropertyOrConstantDto result = sut.Build();
        Assert.That(result, Is.TypeOf<ConstantDto>());
        Assert.That(result.InnerValue(), Is.EqualTo(42));
    }

    [Test]
    public void InnerValue_Constant_GetsConstantValue()
    {
        PropertyOrConstant<TestModel, int> sut = 42;
        Assert.That(sut.InnerValue(), Is.EqualTo(42));
    }

    [Test]
    public void InnerValue_Property_GetsPropertyNameAsString()
    {
        PropertyOrConstant<TestModel, int> sut = new(x => x.Age);
        Assert.That(sut.InnerValue(), Is.EqualTo(nameof(TestModel.Age)));
    }
}
