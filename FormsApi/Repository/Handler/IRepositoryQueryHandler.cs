using System;

namespace FormsApi.Repository.Handler;

public interface IRepositoryQueryHandler<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(string id);
}
