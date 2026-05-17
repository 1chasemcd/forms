using FormsApi.Contract.View;
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
        var view = new CombinedView<TestModel>("Test");
        Assert.That(view.Title.InnerValue(), Is.EqualTo("Test"));
    }

    [Test]
    public void WithPropertyTitle_SetsTitle()
    {
        var view = new CombinedView<TestModel>(m => m.TitleProperty);
        Assert.That(view.Title.InnerValue(), Is.EqualTo(nameof(TestModel.TitleProperty)));
    }

    [Test]
    public void Unify_SetsIsUnified()
    {
        var view = new CombinedView<TestModel>();
        Assert.That(view.IsUnified, Is.False);
        view.Unify();
        Assert.That(view.IsUnified, Is.True);

    }

    [Test]
    public void WithViews_SetsViews()
    {
        IFormView<TestModel> internalView = new Mock<IFormView<TestModel>>().Object;
        var view = new CombinedView<TestModel>()
        {
            internalView
        };

        Assert.That(view.Views.Single(), Is.EqualTo(internalView));
    }

    [Test]
    public void Build_SetsUnifyOnDto()
    {
        var view = new CombinedView<TestModel>();
        view.Unify();
        var built = (CombinedViewDto)view.Build();
        Assert.That(built.Unify, Is.True);
    }

    [Test]
    public void Build_SetsViewsOnDto()
    {
        var internalView = new Mock<IFormView<TestModel>>();
        BaseViewDto viewDto = new Mock<BaseViewDto>().Object;
        internalView.Setup(x => x.Build()).Returns(viewDto);
        var view = new CombinedView<TestModel>()
        {
            internalView.Object
        };

        var built = (CombinedViewDto)view.Build();
        Assert.That(built.Views.Single(), Is.EqualTo(viewDto));
    }
}
