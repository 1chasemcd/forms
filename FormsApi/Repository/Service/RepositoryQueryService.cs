using System;
using FormsApi.Repository.Handler;
using FormsApi.Repository.Query;

namespace FormsApi.Repository.Service;

public class RepositoryQueryService<T>(IRepositoryQueryHandler<T> repository, QueryCriteria criteria) : IRepositoryCallable
{
    public async Task<object> Invoke()
    {
        return await repository.QueryAsync(criteria);
    }
}
