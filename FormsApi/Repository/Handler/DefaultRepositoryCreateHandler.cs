using FormsApi.Repository.Query;

namespace FormsApi.Repository.Handler;

internal sealed class DefaultRepositoryCreateHandler<T> : IRepositoryCreateHandler<T>
    where T : new()
{
    public async Task<T> CreateAsync() => new();
}
