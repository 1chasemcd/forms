using FormsApi.Contract;
using FormsApi.Forms;

namespace Tests.Forms;

[TestFixture]
public class FieldViewTests
{
    private sealed class TestModel
    {
        public string? TitleProperty { get; set; }
    }

    [Test]
    public void WithConstantTitle_SetsTitle()
    {
        var view = new FieldViewBuilder<TestModel>("Test");
        Assert.That(view.Title.InnerValue(), Is.EqualTo("Test"));
    }

    [Test]
    public void WithPropertyTitle_SetsTitle()
    {
        var view = new FieldViewBuilder<TestModel>(m => m.TitleProperty);
        Assert.That(view.Title.InnerValue(), Is.EqualTo(nameof(TestModel.TitleProperty)));
    }

    [Test]
    public void WithFields_SetsFields()
    {
        var view = new FieldViewBuilder<TestModel>()
        {
            { m => m.TitleProperty, 6 }
        };

        Assert.That(view.FieldList.Single(),
            Has.Property(nameof(FormFieldInfoContainer.Identifier))
            .EqualTo(nameof(TestModel.TitleProperty))
            .And.Property(nameof(FormFieldInfoContainer.Width))
            .EqualTo(6));
    }
}
