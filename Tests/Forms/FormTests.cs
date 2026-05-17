using FormsApi.Contract.View;
using FormsApi.Forms;
using Moq;

namespace Tests.Forms;

[TestFixture]
public class FormTests
{
    public sealed class TestModel;

    private sealed class TestForm(IFormView<TestModel> view) : Form<TestModel>
    {
        private IFormView<TestModel> ViewInstance { get; } = view;
        protected override IFormView<TestModel> View => ViewInstance;
    }

    [Test]
    public void GetModelType_ReturnsGenericModelType()
    {
        var view = new Mock<IFormView<TestModel>>();

        var sut = new TestForm(view.Object);

        Type result = sut.GetModelType();

        Assert.That(result, Is.EqualTo(typeof(TestModel)));
    }

    [Test]
    public void GetView_ReturnsBuiltView()
    {
        BaseViewDto expectedView = new Mock<BaseViewDto>().Object;

        var view = new Mock<IFormView<TestModel>>();

        view.Setup(x => x.Build())
            .Returns(expectedView);

        var sut = new TestForm(view.Object);

        BaseViewDto result = sut.GetView();

        Assert.That(result, Is.EqualTo(expectedView));
    }

    [Test]
    public void GetView_CallsBuildOnView()
    {
        BaseViewDto expectedView = new Mock<BaseViewDto>().Object;

        var view = new Mock<IFormView<TestModel>>();

        view.Setup(x => x.Build())
            .Returns(expectedView);

        var sut = new TestForm(view.Object);

        sut.GetView();

        view.Verify(x => x.Build(), Times.Once);
    }
}
