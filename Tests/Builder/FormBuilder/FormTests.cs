using FormsApi.Common.Registry;
using FormsApi.Form;
using FormsApi.Form.Primitives;

namespace Tests.Builder.FormBuilder;

public class FormTests
{
    [Test]
    public void Build_SetsCorrectRepositoryType()
    {
        FormModel form = new TestFormBuilder().Build();

        RepositoryType? expectedType = new(typeof(TestModel));

        Assert.That(form.Type, Is.EqualTo(expectedType));
    }
}
