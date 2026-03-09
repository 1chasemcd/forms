using System;

namespace FormsApi.Repository.Service;

public interface IRepositoryCallable
{
    Task<object> Invoke();
}
