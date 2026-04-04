using System;

namespace FormsApi.Repository.Handler;

public interface IRepositoryQueryHandler<T>
{
    Task<IQueryable<T>> QueryAsync();
}
