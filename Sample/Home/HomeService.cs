using System.Reflection;
using FormsApi.Contract;
using FormsApi.Contract.PostRequest;
using FormsApi.Repository.Handlers;

namespace Sample.Home;

public class HomeService(ILogger<HomeService> logger) : IRepositoryCreateHandler<TestModel>, IRepositorySaveHandler<TestModel>
{
    public async Task<TestModel> CreateAsync() => new();
    public async Task SaveAsync(TestModel toSave)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Saving model with {count} movies.", toSave.Movies.Count);
    }

    public PostRequestAction? SetNumericValue(TestModel model)
    {
        model.NumericField = 12345;
        return null;
    }

    public PostRequestAction? ResetForm(TestModel model)
    {
        CopyMembers(new TestModel(), model);
        return null;
    }

    private static void CopyMembers<T>(T source, T target)
    {
        Type type = typeof(T);

        foreach (PropertyInfo prop in type.GetProperties())
        {
            if (!prop.CanRead || !prop.CanWrite) continue;

            object? value = prop.GetValue(source);
            prop.SetValue(target, value);
        }

        foreach (FieldInfo field in type.GetFields())
        {
            object? value = field.GetValue(source);
            field.SetValue(target, value);
        }
    }

    public PostRequestAction? Reserialize(TestModel _) => null;
}
