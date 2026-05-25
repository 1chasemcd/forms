using FormsApi.Forms;
using Moq;

namespace Tests.Forms;

[TestFixture]
public class CombinedViewTests
{
    public sealed class TestModel
    {
        public string? TitleProperty { get; set; }
    }

    [Test]
    public void WithConstantTitle_SetsTitle()
    {
        var view = new CombinedViewBuilder<TestModel>("Test");
        Assert.That(view.Title.InnerValue(), Is.EqualTo("Test"));
    }

    [Test]
    public void WithPropertyTitle_SetsTitle()
    {
        var view = new CombinedViewBuilder<TestModel>(m => m.TitleProperty);
        Assert.That(view.Title.InnerValue(), Is.EqualTo(nameof(TestModel.TitleProperty)));
    }

    [Test]
    public void Unify_SetsIsUnified()
    {
        var view = new CombinedViewBuilder<TestModel>();
        Assert.That(view.IsUnified, Is.False);
        view.Unify();
        Assert.That(view.IsUnified, Is.True);

    }

    [Test]
    public void WithViews_SetsViews()
    {
        IViewBuilder<TestModel> internalView = new Mock<IViewBuilder<TestModel>>().Object;
        var view = new CombinedViewBuilder<TestModel>()
        {
            internalView
        };

        Assert.That(view.Views.Single(), Is.EqualTo(internalView));
    }
}
