using FormsApi.Form;
using FormsApi.Repository;

namespace Tests.Builder.FormBuilder;

public class FormTests
{
    private readonly FormModel _form = new TestFormBuilder().Build();

    [Test]
    public void Build_SetsCorrectRepositoryType()
    {
        var expectedType = new RepositoryType(typeof(TestModel));
        Assert.That(_form.Type, Is.EqualTo(expectedType));
    }
}
