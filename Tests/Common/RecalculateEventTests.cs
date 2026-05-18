using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract;
using FormsApi.Recalculate;

namespace Tests.Common;

[TestFixture]
public class RecalculateEventTests
{
    private sealed class TestModel;
    private sealed class TestService
    {
        public PostRecalculateEvent? ValidMethod(TestModel _) => null!;
    }

    [Test]
    public void Build_WhenExpressionIsValid_ReturnsRecalculateEventDto()
    {
        var sut = new RecalculateEvent<TestModel, TestService>(x => x.ValidMethod);

        RecalculateEventDto result = sut.Build();

        Assert.That(result.Service, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Service.GetRuntimeType(), Is.EqualTo(typeof(TestService)));
            Assert.That(result.Method, Is.EqualTo(nameof(TestService.ValidMethod)));
        }

    }

    [Test]
    public void Build_WhenExpressionIsNotMethodSelector_ThrowsInvalidOperationException()
    {
        Expression<Func<TestService, Func<TestModel, PostRecalculateEvent?>>> expression =
            x => _ => null;

        var sut = new RecalculateEvent<TestModel, TestService>(expression);

        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(() => sut.Build());

        Assert.That(
            ex!.Message,
            Does.Contain("must be a method selector"));
    }
}
