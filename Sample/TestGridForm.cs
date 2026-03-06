using System;
using FormsApi.Builder;
using FormsApi.Builder.View;

namespace Sample;

public class TestGridForm : FormBuilder<TestModel>
{
    protected override ViewBuilder<TestModel> View => CreateMovieGridView();

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
