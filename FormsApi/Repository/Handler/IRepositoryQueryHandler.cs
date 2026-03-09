using System;
using FormsApi.Repository.Query;

namespace FormsApi.Repository.Handler;

public interface IRepositoryQueryHandler<T>
{
    Task<IEnumerable<T>> QueryAsync(QueryCriteria criteria);
}
