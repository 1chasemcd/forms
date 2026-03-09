using System;

namespace FormsApi.Repository.Handler;

public interface IRepositoryCreateHandler<T>
{
    Task<T> CreateAsync();
}
