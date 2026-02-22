using FormsApi.Common.Types;

namespace Sample;

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

    public string TextField { get; set; } = string.Empty;
    public DateOnly DateField { get; set; }
    public decimal NumericField { get; set; }
    public Currency CurrencyField { get; set; } = 0;

    public IList<Movie> Movies { get; set; } = [];
}


public class Movie
{
    public string? Name { get; init; }
    public string? DirectorName { get; init; }
    public DateOnly ReleaseDate { get; init; }
    public int MyPersonalRating { get; set; }
}
