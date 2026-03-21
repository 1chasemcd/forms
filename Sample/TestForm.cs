using FormsApi.Builder;
using FormsApi.Builder.Field;
using FormsApi.Builder.View;

namespace Sample;

public class TestForm : FormBuilder<TestModel>
{
    protected override ViewBuilder<TestModel> View => new CombinedViewBuilder<TestModel>("A Sample Form")
    {
        TopLeftView(),
        TopRightView(),
        BottomView(),
        GridView()
    };

    private static ViewBuilder<TestModel> TopLeftView()
    {
        return new DataViewBuilder<TestModel>(width: 4)
        {
            { m => m.TextField, p => p.WithWidth(6).WithLabel(m => m.SetTheLabelOnAnotherField) },
            { m => m.DateField, p => p.WithWidth(6) },
            { m => Button.OnModel(m).WithRecalculate<TestService>(s => s.ResetForm) },
            { m => Button.OnModel(m).WithRecalculate<TestService>(s => s.SetNumericValue) },
        };
    }

    private static ViewBuilder<TestModel> TopRightView()
    {
        return new DataViewBuilder<TestModel>(title: "Additional Fields", width: 8)
        {
            {m => m.CurrencyField, p => p.WithRequired() },
            m => m.TextFieldWithInitialValue
        };
    }

    private static ViewBuilder<TestModel> BottomView()
    {
        return new DataViewBuilder<TestModel>()
        {
            {m => m.NumericField, p => p.WithWidth(6).WithRecalculate<TestService>(s => s.Reserialize)},
            {m => m.ResultNumberPlus1, p => p.WithWidth(6)},
            m => m.CheckBox,
            "A static message to display at the bottom",
            m => m.AdditionalMessage,
            {m => m.SetTheLabelOnAnotherField, p => p.WithDisabled(m => m.CheckBox) },
            m => m.TextAreaInput,
            m => m.TimeInput
        };
    }

    private static SubPropertyGridViewBuilder<TestModel, Movie> GridView()
    {
        return new SubPropertyGridViewBuilder<TestModel, Movie>(m => m.Movies)
        {
            { m => m.Name, p => p.Width = 6 },
            m => m.ReleaseDate,
            m => m.DirectorName,
            m => m.MyPersonalRating
        };
    }
}
