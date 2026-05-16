using FormsApi.Definition;
using FormsApi.Definition.Input;
using FormsApi.Definition.View;

namespace Tests.Builder.FormBuilder;

public class SubPropertyGridTests
{
    private readonly FormDto _form = new TestFormBuilder().Build();
    private SubPropertyGridViewDefinition GridView => ((CombinedViewDto)_form.View).Views
        .Select(x => x as SubPropertyGridViewDefinition).Where(x => x != null).ToList()[0]!;


    [Test]
    public void Build_SetsCorrectSubPropertyName()
    {
        Assert.That(GridView.SubPropertyName, Is.EqualTo(nameof(TestModel.EnumerableProperty)));
    }

    [TestCase(nameof(TestModel.TestModelChild.Property1), 0)]
    [TestCase(nameof(TestModel.TestModelChild.Property2), 1)]
    public void SubPropertyGridView_MaintainsCorrectColumnOrder(string propertyName, int expectedIndex)
    {
        Assert.That(GridView.Fields, Is.Not.Null);
        Assert.That(GridView.Fields.ToList(), Has.ItemAt(expectedIndex)
            .With.Property(nameof(FieldDto.Property)).EqualTo(propertyName));
    }
}
