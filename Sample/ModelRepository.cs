using System;
using FormsApi.Repository;
using FormsApi.Repository.Query;

namespace Sample;

public class ModelRepository : IRepository<TestModel>
{
    public Task DeleteAsync(TestModel toDelete) => throw new NotImplementedException();
    public Task<IEnumerable<TestModel>> GetAsync(QueryCriteria criteria) => throw new NotImplementedException();
    public async Task<TestModel> GetNewAsync() => new();
    public async Task SaveAsync(TestModel toSave)
    {

    }
}
