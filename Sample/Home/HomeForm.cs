using FormsApi.Forms;

namespace Sample.Home;

public class HomeForm : Form<TestModel>
{
    protected override IViewBuilder<TestModel> View => new CombinedViewBuilder<TestModel>()
    {
        { TopLeftView(), 4 },
        { TopRightView(), 8 },
        NestedCombinedView(),
        TableView()
    };

    private static FieldViewBuilder<TestModel> TopLeftView()
    {
        return new FieldViewBuilder<TestModel>("Top left view")
        {
            { m => m.TextField, 6 },
            { m => m.DateField, 6 },
            { m => m.ResetForm },
            { m => m.SetNumericValue }
        };
    }

    private static FieldViewBuilder<TestModel> TopRightView()
    {
        return new FieldViewBuilder<TestModel>(title: "Additional Fields")
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

    private static FieldViewBuilder<TestModel> BottomView()
    {
        return new FieldViewBuilder<TestModel>()
        {
            { m => m.NumericField, 6 },
            { m => m.ResultNumberPlus1, 6 },
            m => m.SetTheLabelOnAnotherField,
            m => m.TextAreaInput,
            m => m.TimeInput
        };
    }

    private static FieldViewBuilder<TestModel> BottomViewRight()
    {
        return new FieldViewBuilder<TestModel>("Inner View")
        {
            m => m.CheckBox,
            m => m.StaticTextAtTheBottom,
            m => m.AdditionalMessage,
        };
    }

    private static SubPropertyTableViewBuilder<TestModel, Movie> TableView()
    {
        return new SubPropertyTableViewBuilder<TestModel, Movie>(m => m.Movies, r => r.Name)
        {
            { m => m.Name, 6},
            { m => m.ReleaseDate },
            { m => m.DirectorName },
            m => m.MyPersonalRating
        }.WithTitle("A Table View").EnableEdit();
    }
}
