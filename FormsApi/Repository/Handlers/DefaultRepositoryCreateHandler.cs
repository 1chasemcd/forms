namespace FormsApi.Repository.Handlers;

internal sealed class DefaultRepositoryCreateHandler<T> : IRepositoryCreateHandler<T>
    where T : new()
{
    public async Task<T> CreateAsync() => new();
}
