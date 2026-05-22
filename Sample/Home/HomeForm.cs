using FormsApi.Forms;

namespace Sample.Home;

public class HomeForm : Form<TestModel>
{
    protected override IView<TestModel> View => new CombinedView<TestModel>()
    {
        { TopLeftView(), 4 },
        { TopRightView(), 8 },
        NestedCombinedView(),
        GridView()
    };

    private static ControlView<TestModel> TopLeftView()
    {
        return new ControlView<TestModel>("Top left view")
        {
            { m => m.TextField, 6 },
            { m => m.DateField, 6 },
            { m => m.ResetForm },
            { m => m.SetNumericValue }
        };
    }

    private static ControlView<TestModel> TopRightView()
    {
        return new ControlView<TestModel>(title: "Additional Fields")
        {
            m => m.CurrencyField,
            m => m.TextFieldWithInitialValue
        };
    }

    private static CombinedView<TestModel> NestedCombinedView()
    {
        return new CombinedView<TestModel>()
        {
            { BottomView(), 8 },
            { BottomViewRight(), 4 }
        }.Unify();
    }

    private static ControlView<TestModel> BottomView()
    {
        return new ControlView<TestModel>()
        {
            { m => m.NumericField, 6 },
            { m => m.ResultNumberPlus1, 6 },
            m => m.SetTheLabelOnAnotherField,
            m => m.TextAreaInput,
            m => m.TimeInput
        };
    }

    private static ControlView<TestModel> BottomViewRight()
    {
        return new ControlView<TestModel>("Inner View")
        {
            m => m.CheckBox,
            m => m.StaticTextAtTheBottom,
            m => m.AdditionalMessage,
        };
    }

    private static SubPropertyGridView<TestModel, Movie> GridView()
    {
        return new SubPropertyGridView<TestModel, Movie>(m => m.Movies, r => r.Name)
        {
            { m => m.Name, 6},
            { m => m.ReleaseDate },
            { m => m.DirectorName },
            m => m.MyPersonalRating
        }.WithTitle("A Grid View");
    }
}
