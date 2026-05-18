using FormsApi.Forms;

namespace Sample.Home;

public class HomeForm : Form<TestModel>
{
    protected override IView<TestModel> View => new CombinedView<TestModel>()
    {
        TopLeftView(),
        TopRightView(),
        NestedCombinedView(),
        GridView()
    };

    private static ControlView<TestModel> TopLeftView()
    {
        return new ControlView<TestModel>("Top left view")
        {
            Width = 4,
            Fields = new ControlList<TestModel>
            {
                { m => m.TextField, 6 },
                { m => m.DateField, 6 },
                { m => m.ResetForm },
                { m => m.SetNumericValue }
            }
        };
    }

    private static ControlView<TestModel> TopRightView()
    {
        return new ControlView<TestModel>(title: "Additional Fields")
        {
            m => m.CurrencyField,
            m => m.TextFieldWithInitialValue
        }.WithWidth(8).Disabled();
    }

    private static CombinedView<TestModel> NestedCombinedView()
    {
        return new CombinedView<TestModel>()
        {
            BottomView(),
            BottomViewRight()
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
        }.WithWidth(8);
    }

    private static ControlView<TestModel> BottomViewRight()
    {
        return new ControlView<TestModel>("Inner View")
        {
            m => m.CheckBox,
            m => m.StaticTextAtTheBottom,
            m => m.AdditionalMessage,
        }.WithWidth(4);
    }

    private static SubPropertyGridView<TestModel, Movie> GridView()
    {
        var view = new SubPropertyGridView<TestModel, Movie>(m => m.Movies, r => r.Name)
        {
            { m => m.Name, 6},
            { m => m.ReleaseDate },
            { m => m.DirectorName },
            m => m.MyPersonalRating
        };

        view.Title = "A Grid View";
        return view;
    }
}
