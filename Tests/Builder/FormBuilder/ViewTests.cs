using FormsApi.Definition;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;

namespace Tests.Builder.FormBuilder;

public class ViewTests
{
    private readonly FormDefinition _form = new TestFormBuilder().Build();

    [Test]
    public void Build_SetsCorrectRootViewType()
    {
        Assert.That(_form.View, Is.InstanceOf<CombinedViewDefinition>());
    }

    [Test]
    public void Build_SetsCorrectViewTitle()
    {
        Assert.That(_form.View.Title, Is.EqualTo(new Constant("Title")));
    }
}
