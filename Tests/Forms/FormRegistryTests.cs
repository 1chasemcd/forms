using FormsApi.Contract.View;
using FormsApi.Forms;
using FormsApi.Forms.Services;

namespace Tests.Forms;

[TestFixture]
public class FormRegistryTests
{
    private sealed class TestModel;
    private sealed class TestForm : Form<TestModel>
    {
        protected override BaseView<TestModel> View => new FieldView<TestModel>();
    }

    [Test]
    public void AddForm_WhenPathAlreadyExists_ThrowsInvalidOperationException()
    {
        var registry = new FormRegistry();

        var form1 = new TestForm();
        var form2 = new TestForm();

        registry.AddForm("test", form1);

        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(() =>
            registry.AddForm("test", form2));

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

        registry.AddForm("test", new TestForm());

        Tuple<Type, BaseViewDto>? result = registry.TryGet("test");

        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Item1, Is.EqualTo(typeof(TestModel)));
            Assert.That(result.Item2, Is.InstanceOf<FieldViewDto>());
        }

    }
}
