namespace FormsApi.Repository.Service;

internal interface IWriteableRepositoryService
{
    Task SaveAsync();
    Task DeleteAsync();

}

internal sealed class WriteableRepositoryService<T>(
    IRepository<T> repository,
    T obj
) : IWriteableRepositoryService
{
    public async Task DeleteAsync()
    {
        await repository.DeleteAsync(obj);
    }
    public async Task SaveAsync()
    {
        await repository.SaveAsync(obj);
    }
}
