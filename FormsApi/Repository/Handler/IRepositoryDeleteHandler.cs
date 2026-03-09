using System;

namespace FormsApi.Repository.Handler;

public interface IRepositoryDeleteHandler<T>
{
    Task DeleteAsync(T toDelete);
}
