using FormsApi.Common;
using FormsApi.Contract;

namespace Tests.Common;

[TestFixture]
public class FormValueRefTests
{
    private sealed class TestModel
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    [Test]
    public void Build_WhenConstructedWithPropertySelector_ReturnsPropertyDto()
    {
        var sut = new FormValueRefBuilder<TestModel, string>(x => x.Name);
        FormValueRef result = sut.Build();
        Assert.That(result, Is.TypeOf<ModelValue>());
        Assert.That(result.InnerValue(), Is.EqualTo(nameof(TestModel.Name)));
    }

    [Test]
    public void Build_WhenConstructedWithConstantValue_ReturnsConstantDto()
    {
        var sut = new FormValueRefBuilder<TestModel, string>("test");
        FormValueRef result = sut.Build();
        Assert.That(result, Is.TypeOf<ConstantValue>());
        Assert.That(result.InnerValue(), Is.EqualTo("test"));
    }

    [Test]
    public void Build_WhenConstructedUsingImplicitConstantConversion_ReturnsConstantDto()
    {
        FormValueRefBuilder<TestModel, int> sut = 42;
        FormValueRef result = sut.Build();
        Assert.That(result, Is.TypeOf<ConstantValue>());
        Assert.That(result.InnerValue(), Is.EqualTo(42));
    }

    [Test]
    public void InnerValue_Constant_GetsConstantValue()
    {
        FormValueRefBuilder<TestModel, int> sut = 42;
        Assert.That(sut.InnerValue(), Is.EqualTo(42));
    }

    [Test]
    public void InnerValue_Property_GetsPropertyNameAsString()
    {
        FormValueRefBuilder<TestModel, int> sut = new(x => x.Age);
        Assert.That(sut.InnerValue(), Is.EqualTo(nameof(TestModel.Age)));
    }
}
