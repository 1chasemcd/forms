using FormsApi.Form;
using FormsApi.Form.View;

namespace Tests.Builder.FormBuilder;

public class ViewTests
{
    private readonly FormModel _form = new TestFormBuilder().Build();

    [Test]
    public void Form_View()
    {
        Assert.That(_form.View, Is.InstanceOf<CombinedView>());
    }

    [Test]
    public void View_Title()
    {
        Assert.That(_form.View.Title, Is.EqualTo(new Constant<string>("Title")));
    }
}
