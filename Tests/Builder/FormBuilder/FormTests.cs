using FormsApi.Definition;
using FormsApi.Definition.Primitives;

namespace Tests.Builder.FormBuilder;

public class FormTests
{
    [Test]
    public void Build_SetsCorrectRepositoryType()
    {
        FormDefinition form = new TestFormBuilder().Build();

        SerializedType? expectedType = new(typeof(TestModel));

        Assert.That(form.ModelType, Is.EqualTo(expectedType));
    }
}
