using FormsApi.Common.Registry;
using FormsApi.Form;
using FormsApi.Form.Primitives;

namespace Tests.Builder.FormBuilder;

public class FormTests
{
    private readonly RepositoryTypeRegistry _reg = new();

    [Test]
    public void Build_SetsCorrectRepositoryType()
    {
        FormModel form = new TestFormBuilder().Build(_reg);

        RepositoryType? expectedType = _reg.TryGetRepositoryType<TestModel>();
        RepositoryType otherType = _reg.Add<FormTests>();

        Assert.That(form.Type, Is.EqualTo(expectedType));

        // Need to verify that a different type has a different value
        Assert.That(form.Type, Is.Not.EqualTo(otherType));
    }
}
