using FormsApi.Builder;
using FormsApi.Builder.View;
using FormsApi.Form;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;
using FormsApi.Form.View;

namespace Tests.Builder.FormBuilder;

public class ButtonBuilderTests
{
    private FormDefinition _form = new TestBuilder().Build();
    private IEnumerable<BaseField> GetFields()
    {
        return (_form.View as DataView)?.Fields ?? [];
    }
    [Test]
    public void Build_NoLabelSpecified_UsesMethodName()
    {
        IEnumerable<PropertyOrConstant?> labels = GetFields().Select(f => f.Label);
        Assert.That(labels, Has.One.With.Property(nameof(Constant.Value)).EqualTo("This Is A Button"));
    }

    private class TestBuilder : FormBuilder<TestModel>
    {
        protected override ViewBuilder<TestModel> View => new DataViewBuilder<TestModel>()
        {
            m => m.ThisIsAButton
        };
    }

    private class TestModel
    {
        public void ThisIsAButton()
        {

        }
    }
}
