using FormsApi.Metadata;

namespace Sample.Home;

public class TestModelMetadata : Metadata<TestModel>
{
    public TestModelMetadata()
    {
        LabelValue(m => m.StaticTextAtTheBottom);
        LabelValue(m => m.AdditionalMessage);

        Currency(m => m.CurrencyField);

        Numeric(m => m.NumericField)
            .WithMaxValue(100)
            .WithMinValue(50)
            .OnChange(InvokeServiceMethod<HomeService>(s => s.Reserialize));


        Text(m => m.TextField)
            .WithMaxLength(10)
            .WithLabel(m => m.SetTheLabelOnAnotherField)
            .EnabledWhen(m => m.CheckBox);

        TextArea(m => m.TextAreaInput)
            .WithMaxLength(200);

        Button(m => m.ResetForm)
            .OnChange(InvokeServiceMethod<HomeService>(s => s.ResetForm));

        Button(m => m.SetNumericValue)
            .OnChange(InvokeServiceMethod<HomeService>(s => s.SetNumericValue));
    }
}

public class MovieMetadata : Metadata<Movie>
{
    public MovieMetadata()
    {
        Numeric(m => m.MyPersonalRating)
            .WithMinValue(0)
            .WithMaxValue(5);
        Text(m => m.Name).Disabled();
        Text(m => m.DirectorName).Disabled();
        Date(m => m.ReleaseDate).Disabled();
    }
}
