using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Contract;
using FormsApi.FormService;

namespace Tests.Common;

[TestFixture]
public class FormServiceMethodTests
{
    private sealed class TestModel;
    private sealed class TestService
    {
        public FormServicePostAction? ValidMethod(TestModel _) => null!;
    }

    [Test]
    public void Build_WhenExpressionIsValid_ReturnsFormServiceMethodDto()
    {
        var sut = new FormServiceMethod<TestModel, TestService>(x => x.ValidMethod);

        FormServiceMethodDto result = sut.Build();

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
        Expression<Func<TestService, Func<TestModel, FormServicePostAction?>>> expression =
            x => _ => null;

        var sut = new FormServiceMethod<TestModel, TestService>(expression);

        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(() => sut.Build());

        Assert.That(
            ex!.Message,
            Does.Contain("must be a method selector"));
    }
}
