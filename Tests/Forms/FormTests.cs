using FormsApi.Contract.View;
using FormsApi.Forms;
using FormsApi.Forms.Services;
using Moq;

namespace Tests.Forms;

[TestFixture]
public class FormTests
{
    public sealed class TestModel;

    private sealed class TestForm : Form<TestModel>
    {
        protected internal override IViewBuilder<TestModel> View => null!;
    }

    [Test]
    public void ModelType_ReturnsGenericModelType()
    {
        var sut = new TestForm();

        Type result = sut.ModelType;

        Assert.That(result, Is.EqualTo(typeof(TestModel)));
    }

    [Test]
    public void ProvideBuilder_CallsBuildOnBuilder()
    {
        var builder = new Mock<IFormBuilderService>();
        var sut = new TestForm();
        var expectedResult = new List<View>();

        builder.Setup(x => x.BuildFormIntoViews(sut))
            .Returns(expectedResult);

        IReadOnlyList<View> result = sut.ProvideBuilder(builder.Object);

        builder.Verify(x => x.BuildFormIntoViews(sut), Times.Once);
        Assert.That(result, Is.SameAs(expectedResult));
    }
}
