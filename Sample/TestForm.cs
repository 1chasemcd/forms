using FormsApi.Builder;
using FormsApi.Builder.Field;
using FormsApi.Builder.View;

namespace Sample;

public class TestForm : FormBuilder<TestModel>
{
    protected override ViewBuilder<TestModel> View => new CombinedViewBuilder<TestModel>()
    {
        TopLeftView(),
        TopRightView(),
        NestedCombinedView(),
        GridView()
    };

    private static ViewBuilder<TestModel> TopLeftView()
    {
        return new FieldViewBuilder<TestModel>("Top left view", width: 4)
        {
            { m => m.TextField, p =>
            {
                p.Width = 6;
                p.Label = Property(m => m.SetTheLabelOnAnotherField);
            } },
            { m => m.DateField, p => p.Width = 6 },
            { m => m.ResetForm, p => p.AddRecalc<TestService>(s => s.ResetForm) },
            { m => m.SetNumericValue, p => p.AddRecalc<TestService>(s => s.SetNumericValue) },
        };
    }

    private static ViewBuilder<TestModel> TopRightView()
    {
        return new FieldViewBuilder<TestModel>(title: "Additional Fields", width: 8)
        {
            {m => m.CurrencyField, p => p.Required = true },
            m => m.TextFieldWithInitialValue
        };
    }

    private static CombinedViewBuilder<TestModel> NestedCombinedView()
    {
        return new CombinedViewBuilder<TestModel>(unify: true)
        {
        BottomView(),
        BottomViewRight()
        };
    }

    private static ViewBuilder<TestModel> BottomView()
    {
        return new FieldViewBuilder<TestModel>(width: 8)
        {
            {m => m.NumericField, p =>
                {
                    p.Width = 6;
                    p.AddRecalc<TestService>(s => s.Reserialize);
                }
            },
            { m => m.ResultNumberPlus1, p => p.Width = 6},
            { m => m.SetTheLabelOnAnotherField, p => p.Enabled = Property(m => m.CheckBox) },
            m => m.TextAreaInput,
            m => m.TimeInput
        };
    }

    private static ViewBuilder<TestModel> BottomViewRight()
    {
        return new FieldViewBuilder<TestModel>("Inner View", width: 4)
        {
            m => m.CheckBox,
            m => m.StaticTextAtTheBottom,
            m => m.AdditionalMessage,
        };
    }

    private static SubPropertyGridViewBuilder<TestModel, Movie> GridView()
    {
        var view = new SubPropertyGridViewBuilder<TestModel, Movie>(m => m.Movies, r => r.Name)
        {
            { m => m.Name, p =>
            {
                p.Width = 6;
                p.Enabled = false;
            } },
            { m => m.ReleaseDate, p => p.Enabled = false },
            { m => m.DirectorName, p => p.Enabled = false },
            m => m.MyPersonalRating
        };

        view.Title = "A Grid View";
        return view;
    }
}
