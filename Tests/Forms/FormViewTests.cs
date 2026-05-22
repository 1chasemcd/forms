using FormsApi.Contract.View;
using FormsApi.Forms;

namespace Tests.Forms;

[TestFixture]
public class FormViewTests
{
    private sealed class TestModel
    {
        public string? StringProperty { get; set; }
        public bool BoolProperty { get; set; }
    }
    private sealed class TestFormView() : View<TestModel, TestFormView>;

    [Test]
    public void WithTitle_AsConstant_SetsTitle()
    {
        TestFormView view = new TestFormView().WithTitle("Test");
        Assert.That(view.Title.InnerValue(), Is.EqualTo("Test"));
    }

    [Test]
    public void WithTitle_AsProperty_SetsTitle()
    {
        TestFormView view = new TestFormView().WithTitle(x => x.StringProperty);
        Assert.That(view.Title.InnerValue(), Is.EqualTo(nameof(TestModel.StringProperty)));
    }

    [Test]
    public void WithWidth_SetsWidth()
    {
        TestFormView view = new TestFormView().WithWidth(6);
        Assert.That(view.Width, Is.EqualTo(6));
    }

    [Test]
    public void Hidden_SetsVisibleToFalse()
    {
        TestFormView view = new TestFormView().Hidden();
        Assert.That(view.Visible.InnerValue(), Is.False);
    }

    [Test]
    public void VisibleWhen_SetsVisible()
    {
        TestFormView view = new TestFormView().VisibleWhen(x => x.BoolProperty);
        Assert.That(view.Visible.InnerValue(), Is.EqualTo(nameof(TestModel.BoolProperty)));
    }
}
