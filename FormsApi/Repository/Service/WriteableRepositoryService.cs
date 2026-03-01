namespace FormsApi.Repository.Service;

internal interface IWriteableRepositoryService
{
    Task SaveAsync();
    Task DeleteAsync();

}

internal sealed class WriteableRepositoryService<TRepo, TObj>(
    IRepository<TRepo> repository,
    TObj obj) : IWriteableRepositoryService
    where TObj : TRepo
{
    internal IRepository<TRepo> Repository { get; init; } = repository;
    internal TObj Object { get; init; } = obj;
    public async Task DeleteAsync()
    {
        await Repository.DeleteAsync(Object);
    }
    public async Task SaveAsync()
    {
        await Repository.SaveAsync(Object);
    }
}
