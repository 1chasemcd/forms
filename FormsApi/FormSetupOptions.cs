using FormsApi.Forms;
using FormsApi.Repository.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi;

public interface IFormSetupOptions
{
    public IFormSetupOptions AddForm<TForm>(string path)
        where TForm : IForm;
    IFormSetupOptions AddRepository<TRepository>(ServiceLifetime lifetime = ServiceLifetime.Singleton);
}
internal sealed class FormSetupOptions(IServiceCollection services) : IFormSetupOptions
{
    private readonly ICollection<KeyValuePair<string, Type>> _builders = [];
    public IFormSetupOptions AddForm<TForm>(string path)
        where TForm : IForm
    {
        _builders.Add(new(path, typeof(TForm)));
        return this;
    }

    internal IEnumerable<KeyValuePair<string, IForm>> GetForms()
    {
        foreach (KeyValuePair<string, Type> registration in _builders)
        {
            IForm? builder = Activator.CreateInstance(registration.Value) as IForm;
            if (builder is not null)
                yield return new(registration.Key, builder);
            else
                throw new InvalidOperationException($"Unable to build form {registration.Value.Name}");
        }
    }

    public IFormSetupOptions AddRepository<TRepository>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        Type[] repositoryOptions = [
            typeof(IRepositoryCreateHandler<>),
            typeof(IRepositorySaveHandler<>),
            typeof(IRepositoryDeleteHandler<>),
            typeof(IRepositoryQueryHandler<>)
            ];

        var repoInterfaces = typeof(TRepository)
            .GetInterfaces()
            .Where(i =>
                i.IsGenericType &&
                repositoryOptions.Contains(i.GetGenericTypeDefinition()))
            .ToList();

        if (repoInterfaces.Count == 0)
            throw new InvalidOperationException(
                $"{typeof(TRepository).Name} does not implement any of {repositoryOptions}");


        foreach (Type repoInterface in repoInterfaces)
        {
            CheckAlreadyRegistered(services, repoInterface);
            services.Add(new ServiceDescriptor(repoInterface, typeof(TRepository), lifetime));
        }
        return this;
    }

    private void CheckAlreadyRegistered(IServiceCollection services, Type repoInterface)
    {
        ServiceDescriptor? existing = services.FirstOrDefault(s => s.ServiceType == repoInterface);

        if (existing == null)
            return;

        throw new InvalidOperationException(
            $"Repository for {repoInterface} already registered: " +
            $"{existing.ImplementationType?.Name}");
    }
}
