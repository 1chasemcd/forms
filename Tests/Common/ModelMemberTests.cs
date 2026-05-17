using FormsApi.Common;

namespace Tests.Common;

[TestFixture]
public class ModelMemberTests
{
    private sealed class TestModel
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public NestedModel? Nested { get; set; }

        public string GetValue() => "test";
    }

    private sealed class NestedModel
    {
        public string? Value { get; set; }
    }

    [Test]
    public void Build_WhenExpressionIsDirectReferenceTypeProperty_ReturnsPropertyName()
    {
        var sut = new ModelMember<TestModel, string>(x => x.Name);
        string result = sut.Build();
        Assert.That(result, Is.EqualTo(nameof(TestModel.Name)));
    }

    [Test]
    public void Build_WhenExpressionIsDirectValueTypeProperty_ReturnsPropertyName()
    {
        var sut = new ModelMember<TestModel, int>(x => x.Age);
        string result = sut.Build();
        Assert.That(result, Is.EqualTo(nameof(TestModel.Age)));
    }

    [Test]
    public void Build_WhenExpressionIsNestedProperty_ThrowsInvalidOperationException()
    {
        var sut = new ModelMember<TestModel, string>(x => x.Nested!.Value);
        var ex = Assert.Throws<InvalidOperationException>(() => sut.Build());
        Assert.That(
            ex!.Message,
            Does.Contain("must access a direct property"));
    }

    [Test]
    public void Build_WhenExpressionIsMethodCall_ThrowsInvalidOperationException()
    {
        var sut = new ModelMember<TestModel, string>(x => x.GetValue());
        var ex = Assert.Throws<InvalidOperationException>(() => sut.Build());
        Assert.That(
            ex!.Message,
            Does.Contain("must be a property access"));
    }

    [Test]
    public void Build_WhenExpressionIsNotPropertyAccess_ThrowsInvalidOperationException()
    {
        var sut = new ModelMember<TestModel, string>(x => "constant");
        var ex = Assert.Throws<InvalidOperationException>(() => sut.Build());
        Assert.That(
            ex!.Message,
            Does.Contain("must be a property access"));
    }
}
