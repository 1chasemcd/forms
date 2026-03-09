using FormsApi.Repository;
using FormsApi.Repository.Handler;
using FormsApi.Repository.Query;

namespace Sample;

public class ModelRepository(ILogger<ModelRepository> logger) : IRepositoryCreateHandler<TestModel>, IRepositorySaveHandler<TestModel>
{
    public async Task<TestModel> CreateAsync() => new();
    public async Task SaveAsync(TestModel toSave)
    {
        logger.LogInformation("Saving model with {count} movies.", toSave.Movies.Count);
    }
}
