using FormsApi.Contract.View;
using FormsApi.Forms;
using FormsApi.Forms.Services;
using Moq;

namespace Tests.Forms;

[TestFixture]
public class FormRegistryTests
{
    private sealed class TestModel;

    [Test]
    public void AddForm_WhenPathAlreadyExists_ThrowsInvalidOperationException()
    {
        var registry = new FormRegistry();

        var form1 = new Mock<IForm>();
        var form2 = new Mock<IForm>();

        registry.AddForm("test", form1.Object);

        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(() =>
            registry.AddForm("test", form2.Object));

        Assert.That(
            ex!.Message,
            Is.EqualTo("Already had a registration for path 'test'"));
    }

    [Test]
    public void TryGet_WhenPathDoesNotExist_ReturnsNull()
    {
        var registry = new FormRegistry();

        Tuple<Type, BaseViewDto>? result = registry.TryGet("missing");

        Assert.That(result, Is.Null);
    }

    [Test]
    public void TryGet_WhenPathExists_ReturnsModelTypeAndView()
    {
        var registry = new FormRegistry();

        BaseViewDto view = new Mock<BaseViewDto>().Object;

        var form = new Mock<IForm>();

        form.Setup(x => x.GetModelType())
            .Returns(typeof(TestModel));

        form.Setup(x => x.GetView())
            .Returns(view);

        registry.AddForm("test", form.Object);

        Tuple<Type, BaseViewDto>? result = registry.TryGet("test");

        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result!.Item1, Is.EqualTo(typeof(TestModel)));
            Assert.That(result.Item2, Is.EqualTo(view));
        }

    }
}
