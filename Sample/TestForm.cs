using FormsApi.Builder;
using FormsApi.Builder.View;

namespace Sample;

public class TestForm : FormBuilder<TestModel>
{
    protected override ViewBuilder<TestModel> View => new CombinedViewBuilder<TestModel>("A Sample Form")
    {
        TopLeftView(),
        TopRightView(),
        BottomView()
    };

    private static ViewBuilder<TestModel> TopLeftView()
    {
        return new DataViewBuilder<TestModel>(width: 4)
        {
            { m => m.TextField, p => p.WithWidth(6) },
            { m => m.DateField, p => p.WithWidth(6) },
            { m => m.ButtonToClick },
        };
    }

    private static ViewBuilder<TestModel> TopRightView()
    {
        return new DataViewBuilder<TestModel>(title: "Additional Fields", width: 8)
        {
            m => m.CurrencyField,
            m => m.TextFieldWithInitialValue
        };
    }

    private static ViewBuilder<TestModel> BottomView()
    {
        return new DataViewBuilder<TestModel>()
        {
            m => m.NumericField,
            m => m.CheckBox,
            "A static message to display at the bottom"
        };
    }
}
