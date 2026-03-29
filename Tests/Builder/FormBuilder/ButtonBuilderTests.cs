using FormsApi.Builder;
using FormsApi.Builder.Field;
using FormsApi.Builder.View;
using FormsApi.Common.Types;
using FormsApi.Definition;
using FormsApi.Definition.Field;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.View;
using FormsApi.Recalculate;

namespace Tests.Builder.FormBuilder;

public class ButtonBuilderTests
{
    private readonly FormDefinition _form = new TestBuilder().Build();
    private IEnumerable<BaseField> GetFields()
    {
        return (_form.View as DataView)?.Fields ?? [];
    }
    [Test]
    public void Build_NoLabelSpecified_UsesMethodName()
    {
        IEnumerable<PropertyOrConstant?> labels = GetFields().Select(f => f.Label);
        Assert.That(labels, Has.One.With.Property(nameof(Constant.Value)).EqualTo("This Is a Button"));
    }

    private class TestBuilder : FormBuilder<TestModel>
    {
        protected override ViewBuilder<TestModel> View => new DataViewBuilder<TestModel>()
        {
            { m => Button.OnModel(m).WithRecalculate<TestService>(s => s.ThisIsAButton) }
        };
    }

    private class TestModel;
    private class TestService
    {
        public PostRecalculateEvent? ThisIsAButton(TestModel model) => null;
    }
}
