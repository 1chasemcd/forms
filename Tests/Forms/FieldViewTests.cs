using FormsApi.Contract;
using FormsApi.Contract.View;
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
        var view = new ControlView<TestModel>("Test");
        Assert.That(view.Title.InnerValue(), Is.EqualTo("Test"));
    }

    [Test]
    public void WithPropertyTitle_SetsTitle()
    {
        var view = new ControlView<TestModel>(m => m.TitleProperty);
        Assert.That(view.Title.InnerValue(), Is.EqualTo(nameof(TestModel.TitleProperty)));
    }

    [Test]
    public void WithFields_SetsFields()
    {
        var view = new ControlView<TestModel>()
        {
            { m => m.TitleProperty, 6 }
        };

        Assert.That(view.Fields.Single(),
            Has.Property(nameof(FormControlLayoutDto.PropertyName))
            .EqualTo(nameof(TestModel.TitleProperty))
            .And.Property(nameof(FormControlLayoutDto.Width))
            .EqualTo(6));
    }

    [Test]
    public void Build_SetsFieldsOnDto()
    {
        var view = new ControlView<TestModel>()
        {
            { m => m.TitleProperty, 6 }
        };

        FieldViewDto built = (FieldViewDto)view.Build();

        Assert.That(view.Fields.Single(),
            Has.Property(nameof(FormControlLayoutDto.PropertyName))
            .EqualTo(nameof(TestModel.TitleProperty))
            .And.Property(nameof(FormControlLayoutDto.Width))
            .EqualTo(6));
    }
}
