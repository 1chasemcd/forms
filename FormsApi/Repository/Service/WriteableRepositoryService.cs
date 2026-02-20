namespace FormsApi.Repository.Service;

internal interface IWriteableRepositoryService
{
    Task SaveAsync();
    Task DeleteAsync();

}

internal sealed class WriteableRepositoryService<T>(
    IRepositoryHandler<T> handler,
    T obj
) : IWriteableRepositoryService
{
    public async Task DeleteAsync()
    {
        await handler.DeleteAsync(obj);
    }
    public async Task SaveAsync()
    {
        await handler.SaveAsync(obj);
    }
}
