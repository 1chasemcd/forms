using FormsApi.Builder;
using FormsApi.Builder.View;

namespace Sample;

public class TestForm : FormBuilder<TestModel>
{
    protected override ViewBuilder<TestModel> View => new CombinedViewBuilder<TestModel>("A Sample Form")
    {
        CreateDataView(),
        CreateMovieGridView()
    };

    private static ViewBuilder<TestModel> CreateDataView()
    {
        return new DataViewBuilder<TestModel>()
        {
            { m => m.TextField, p => p.WithWidth(50) },
            { m => m.DateField, p => p.WithWidth(50) },
            { m => m.NumericField, p => p.WithWidth(50) },
            { m => m.CurrencyField, p => p.WithWidth(50) },
        };
    }

    private static ViewBuilder<TestModel> CreateMovieGridView()
    {
        var movieGrid = new SubPropertyGridViewBuilder<TestModel, Movie>(m => m.Movies)
        {
            m => m.Name,
            m => m.DirectorName,
            m => m.ReleaseDate,
            m => m.MyPersonalRating
        };

        movieGrid.Title = "My Movie Ratings";

        return movieGrid;
    }
}
