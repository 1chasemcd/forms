using FormsApi.Contract.View;
using FormsApi.Forms;
using FormsApi.Forms.Services;
using Moq;

namespace Tests.Forms;

[TestFixture]
public class FormRegistryTests
{
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

        IForm? result = registry.TryGet("missing");

        Assert.That(result, Is.Null);
    }

    [Test]
    public void TryGet_WhenPathExists_ReturnsModelTypeAndView()
    {
        var registry = new FormRegistry();

        View view = new Mock<View>().Object;

        IForm form = new Mock<IForm>().Object;

        registry.AddForm("test", form);

        IForm? result = registry.TryGet("test");

        Assert.That(result, Is.SameAs(form));


    }
}
