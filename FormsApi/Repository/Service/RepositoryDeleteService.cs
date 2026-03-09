using System;
using FormsApi.Repository.Handler;

namespace FormsApi.Repository.Service;

public class RepositoryDeleteService<T>(IRepositoryDeleteHandler<T> repository, T model) : IRepositoryCallable
{

    public async Task<object> Invoke()
    {
        await repository.DeleteAsync(model);
        return new object();
    }
}
