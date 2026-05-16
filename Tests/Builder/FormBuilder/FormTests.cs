using FormsApi.Definition;
using FormsApi.Definition.Primitives;

namespace Tests.Builder.FormBuilder;

public class FormTests
{
    [Test]
    public void Build_SetsCorrectRepositoryType()
    {
        FormDto form = new TestFormBuilder().Build();

        TypeDto? expectedType = new(typeof(TestModel));

        Assert.That(form.ModelType, Is.EqualTo(expectedType));
    }
}
