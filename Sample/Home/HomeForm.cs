using FormsApi.Forms;

namespace Sample.Home;

public class HomeForm : Form<TestModel>
{
    protected override IViewBuilder<TestModel> View => new CombinedViewBuilder<TestModel>()
    {
        { TopLeftView(), 4 },
        { TopRightView(), 8 },
        NestedCombinedView(),
        GridView()
    };

    private static ControlViewBuilder<TestModel> TopLeftView()
    {
        return new ControlViewBuilder<TestModel>("Top left view")
        {
            { m => m.TextField, 6 },
            { m => m.DateField, 6 },
            { m => m.ResetForm },
            { m => m.SetNumericValue }
        };
    }

    private static ControlViewBuilder<TestModel> TopRightView()
    {
        return new ControlViewBuilder<TestModel>(title: "Additional Fields")
        {
            m => m.CurrencyField,
            m => m.TextFieldWithInitialValue
        };
    }

    private static CombinedViewBuilder<TestModel> NestedCombinedView()
    {
        return new CombinedViewBuilder<TestModel>()
        {
            { BottomView(), 8 },
            { BottomViewRight(), 4 }
        }.Unify();
    }

    private static ControlViewBuilder<TestModel> BottomView()
    {
        return new ControlViewBuilder<TestModel>()
        {
            { m => m.NumericField, 6 },
            { m => m.ResultNumberPlus1, 6 },
            m => m.SetTheLabelOnAnotherField,
            m => m.TextAreaInput,
            m => m.TimeInput
        };
    }

    private static ControlViewBuilder<TestModel> BottomViewRight()
    {
        return new ControlViewBuilder<TestModel>("Inner View")
        {
            m => m.CheckBox,
            m => m.StaticTextAtTheBottom,
            m => m.AdditionalMessage,
        };
    }

    private static SubPropertyGridViewBuilder<TestModel, Movie> GridView()
    {
        return new SubPropertyGridViewBuilder<TestModel, Movie>(m => m.Movies, r => r.Name)
        {
            { m => m.Name, 6},
            { m => m.ReleaseDate },
            { m => m.DirectorName },
            m => m.MyPersonalRating
        }.WithTitle("A Grid View").EnableEdit();
    }
}
