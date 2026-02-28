using FormsApi.Common.Registry;
using FormsApi.Form;
using FormsApi.Form.Field;
using FormsApi.Form.View;

namespace Tests.Builder.FormBuilder;

public class SubPropertyGridTests
{
    private readonly FormModel _form = new TestFormBuilder().Build();
    private SubPropertyGridView GridView => ((CombinedView)_form.View).Views
        .Select(x => x as SubPropertyGridView).Where(x => x != null).ToList()[0]!;


    [Test]
    public void Build_SetsCorrectSubPropertyName()
    {
        Assert.That(GridView.SubPropertyName, Is.EqualTo(nameof(TestModel.EnumerableProperty)));
    }

    [TestCase(nameof(TestModel.TestModelChild.Property1), 0)]
    [TestCase(nameof(TestModel.TestModelChild.Property2), 1)]
    public void SubPropertyGridView_MaintainsCorrectColumnOrder(string propertyName, int expectedIndex)
    {
        Assert.That(GridView.Columns, Is.Not.Null);
        Assert.That(GridView.Columns.ToList(), Has.ItemAt(expectedIndex)
            .With.Property(nameof(BaseInput.Property)).EqualTo(propertyName));
    }
}
