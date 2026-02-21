using FormsApi.Common.Registry;
using FormsApi.Form;
using FormsApi.Repository;

namespace Tests.Builder.FormBuilder;

public class FormTests
{
    private readonly RepositoryTypeRegistry reg = new RepositoryTypeRegistry();

    [Test]
    public void Build_SetsCorrectRepositoryType()
    {
        FormModel form = new TestFormBuilder().Build(reg);

        var expectedType = reg.TryGetRepositoryType<TestModel>();
        var otherType = reg.Add<FormTests>();

        Assert.That(form.Type, Is.EqualTo(expectedType));

        // Need to verify that a different type has a different value
        Assert.That(form.Type, Is.Not.EqualTo(otherType));
    }
}
