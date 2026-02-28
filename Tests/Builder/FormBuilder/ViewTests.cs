using FormsApi.Common.Registry;
using FormsApi.Form;
using FormsApi.Form.Primitives;
using FormsApi.Form.View;

namespace Tests.Builder.FormBuilder;

public class ViewTests
{
    private readonly FormModel _form = new TestFormBuilder().Build();

    [Test]
    public void Build_SetsCorrectRootViewType()
    {
        Assert.That(_form.View, Is.InstanceOf<CombinedView>());
    }

    [Test]
    public void Build_SetsCorrectViewTitle()
    {
        Assert.That(_form.View.Title, Is.EqualTo(new Constant("Title")));
    }
}
