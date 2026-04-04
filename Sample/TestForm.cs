using FormsApi.Builder;
using FormsApi.Builder.View;
using FormsApi.Common.Types;
using FormsApi.Repository.Handler;

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

public class TestModel
{
    public TestModel()
    {
        Movies.Add(new Movie()
        {
            Name = "The Lion King",
            DirectorName = "Roger Allers",
            ReleaseDate = new DateOnly(1994, 06, 15)
        });

        Movies.Add(new Movie()
        {
            Name = "Forrest Gump",
            DirectorName = "Robert Zemeckis",
            ReleaseDate = new DateOnly(1994, 06, 23)
        });

        Movies.Add(new Movie()
        {
            Name = "Star Wars",
            DirectorName = "George Lucas",
            ReleaseDate = new DateOnly(1977, 05, 25)
        });
    }
    public bool CheckBox { get; set; }
    public LabelValue StaticTextAtTheBottom => "A static message to display at the bottom";
    public Currency CurrencyField { get; set; } = 0;
    public DateOnly? DateField { get; set; } = new DateOnly(2016, 01, 05);
    public decimal NumericField { get; set; }
    public decimal ResultNumberPlus1 => NumericField + 1;
    public string TextField { get; set; } = string.Empty;
    public string TextFieldWithInitialValue { get; set; } = "Test Value";
    public LabelValue AdditionalMessage => "Another static message";
    public string SetTheLabelOnAnotherField { get; set; } = "Text Field";
    public TextArea TextAreaInput { get; set; } = "value in a text area\nnew line";
    public TimeOnly TimeInput { get; set; } = new TimeOnly(2, 15);
    public Button ResetForm { get; set; }
    public Button SetNumericValue { get; set; }

    public IList<Movie> Movies { get; set; } = [];
}


public class Movie
{
    public string? Name { get; init; }
    public string? DirectorName { get; init; }
    public DateOnly ReleaseDate { get; init; }
    public int MyPersonalRating { get; set; }
}

public class ModelRepository(ILogger<ModelRepository> logger) : IRepositoryCreateHandler<TestModel>, IRepositorySaveHandler<TestModel>
{
    public async Task<TestModel> CreateAsync() => new();
    public async Task SaveAsync(TestModel toSave)
    {
        logger.LogInformation("Saving model with {count} movies.", toSave.Movies.Count);
    }
}
