using FormsApi.Builder;
using FormsApi.Builder.Metadata;
using FormsApi.Builder.View;
using FormsApi.Repository.Handler;

namespace Sample;

public class TestForm : Form<TestModel>
{
    protected override BaseView<TestModel> View => new CombinedView<TestModel>()
    {
        TopLeftView(),
        TopRightView(),
        NestedCombinedView(),
        GridView()
    };

    private static FieldView<TestModel> TopLeftView()
    {
        return new FieldView<TestModel>("Top left view")
        {
            Width = 4,
            Fields = new FieldList<TestModel>
            {
                { m => m.TextField, 6 },
                { m => m.DateField, 6 },
                { m => m.ResetForm },
                { m => m.SetNumericValue }
            }
        };
    }

    private static FieldView<TestModel> TopRightView()
    {
        return new FieldView<TestModel>(title: "Additional Fields")
        {
            m => m.CurrencyField ,
            m => m.TextFieldWithInitialValue
        }.WithWidth(8).Disabled();
    }

    private static CombinedView<TestModel> NestedCombinedView()
    {
        return new CombinedView<TestModel>()
        {
            BottomView(),
            BottomViewRight()
        }.Unified();
    }

    private static FieldView<TestModel> BottomView()
    {
        return new FieldView<TestModel>()
        {
            { m => m.NumericField, 6 },
            { m => m.ResultNumberPlus1, 6 },
            m => m.SetTheLabelOnAnotherField,
            m => m.TextAreaInput,
            m => m.TimeInput
        }.WithWidth(8);
    }

    private static FieldView<TestModel> BottomViewRight()
    {
        return new FieldView<TestModel>("Inner View")
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

public class TestModelMetadata : Metadata<TestModel>
{
    public TestModelMetadata()
    {
        LabelValue(m => m.StaticTextAtTheBottom);
        LabelValue(m => m.AdditionalMessage);

        Currency(m => m.CurrencyField);

        Numeric(m => m.NumericField)
            .WithMaxValue(100)
            .WithMinValue(50);


        Text(m => m.TextField)
            .WithMaxLength(10)
            .EnabledWhen(m => m.CheckBox);

        TextArea(m => m.TextAreaInput)
            .WithMaxLength(200);

        Button(m => m.ResetForm)
            .OnChange(Recalculate<TestService>(s => s.ResetForm));

        Button(m => m.SetNumericValue)
            .OnChange(Recalculate<TestService>(s => s.SetNumericValue));
    }
}

public class MovieMetadata : Metadata<Movie>
{
    public MovieMetadata()
    {
        Numeric(m => m.MyPersonalRating)
            .WithMinValue(0)
            .WithMaxValue(5);
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
    public string StaticTextAtTheBottom => "A static message to display at the bottom";
    public decimal CurrencyField { get; set; } = 0;
    public DateOnly? DateField { get; set; } = new DateOnly(2016, 01, 05);
    public decimal NumericField { get; set; }
    public decimal ResultNumberPlus1 => NumericField + 1;
    public string TextField { get; set; } = string.Empty;
    public string TextFieldWithInitialValue { get; set; } = "Test Value";
    public string AdditionalMessage => "Another static message";
    public string SetTheLabelOnAnotherField { get; set; } = "Text Field";
    public string TextAreaInput { get; set; } = "value in a text area\nnew line";
    public TimeOnly TimeInput { get; set; } = new TimeOnly(2, 15);
    public bool ResetForm { get; set; }
    public bool SetNumericValue { get; set; }

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
