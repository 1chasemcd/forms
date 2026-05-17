using FormsApi.Contract;
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
    private sealed class TestFormView() : FormView<TestModel, TestFormView>
    {
        protected override BaseViewDto BuildImpl() => new MockViewDto();
    }

    private sealed record MockViewDto : BaseViewDto;

    [Test]
    public void Build_ReturnsViewFromBuildImpl()
    {
        var view = new TestFormView();
        BaseViewDto result = view.Build();
        Assert.That(result, Is.InstanceOf<MockViewDto>());
    }

    [Test]
    public void Build_SetsAllProperties()
    {
        var view = new TestFormView
        {
            Title = "Test",
            Width = 6,
            Enabled = false,
            Visible = true
        };
        BaseViewDto result = view.Build();
        Assert.That(result.Title.InnerValue(), Is.EqualTo("Test"));
        Assert.That(result.Width, Is.EqualTo(6));
        Assert.That(result.Enabled.InnerValue(), Is.False);
        Assert.That(result.Visible.InnerValue(), Is.True);
    }

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
    public void Disabled_SetsEnabledToFalse()
    {
        TestFormView view = new TestFormView().Disabled();
        Assert.That(view.Enabled.InnerValue(), Is.False);
    }

    [Test]
    public void EnabledWhen_SetsEnabled()
    {
        TestFormView view = new TestFormView().EnabledWhen(x => x.BoolProperty);
        Assert.That(view.Enabled.InnerValue(), Is.EqualTo(nameof(TestModel.BoolProperty)));
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
