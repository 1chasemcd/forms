using System.Linq.Expressions;

namespace Tests.Common;

[TestFixture]
public class ExpressionExtensionTests
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
    public void GetPropertyName_WhenExpressionIsDirectReferenceTypeProperty_ReturnsPropertyName()
    {
        Expression<Func<TestModel, string?>> sut = x => x.Name;
        string result = sut.GetPropertyName();
        Assert.That(result, Is.EqualTo(nameof(TestModel.Name)));
    }

    [Test]
    public void GetPropertyName_WhenExpressionIsDirectValueTypeProperty_ReturnsPropertyName()
    {
        Expression<Func<TestModel, int?>> sut = x => x.Age;
        string result = sut.GetPropertyName();
        Assert.That(result, Is.EqualTo(nameof(TestModel.Age)));
    }

    [Test]
    public void GetPropertyName_WhenExpressionIsNestedProperty_ThrowsInvalidOperationException()
    {
        Expression<Func<TestModel, string?>> sut = x => x.Nested!.Value;
        ArgumentException? ex = Assert.Throws<ArgumentException>(() => sut.GetPropertyName());
        Assert.That(
            ex!.Message,
            Does.Contain("must access a direct property"));
    }

    [Test]
    public void GetPropertyName_WhenExpressionIsMethodCall_ThrowsInvalidOperationException()
    {
        Expression<Func<TestModel, string?>> sut = x => x.GetValue();
        ArgumentException? ex = Assert.Throws<ArgumentException>(() => sut.GetPropertyName());
        Assert.That(
            ex!.Message,
            Does.Contain("must be a property access"));
    }

    [Test]
    public void GetPropertyName_WhenExpressionIsNotPropertyAccess_ThrowsInvalidOperationException()
    {
        Expression<Func<TestModel, string?>> sut = x => "constant";
        ArgumentException? ex = Assert.Throws<ArgumentException>(() => sut.GetPropertyName());
        Assert.That(
            ex!.Message,
            Does.Contain("must be a property access"));
    }
}
