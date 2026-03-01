using FormsApi.Builder;
using FormsApi.Form;
using FormsApi.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi.Common.Registry;

public interface IFormSetupOptions
{
    public IFormSetupOptions AddForm<TBuilder>(string path)
        where TBuilder : FormBuilder;
    IFormSetupOptions AddRepository<TRepository>(ServiceLifetime lifetime = ServiceLifetime.Singleton);
}
internal class FormSetupOptions(IServiceCollection services) : IFormSetupOptions
{
    private readonly ICollection<KeyValuePair<string, Type>> _builders = [];
    public IFormSetupOptions AddForm<TBuilder>(string path)
        where TBuilder : FormBuilder
    {
        _builders.Add(new(path, typeof(TBuilder)));
        return this;
    }

    internal IEnumerable<KeyValuePair<string, FormModel>> BuildForms()
    {
        foreach (KeyValuePair<string, Type> registration in _builders)
        {
            FormModel? form = (Activator.CreateInstance(registration.Value) as FormBuilder)?.Build();
            if (form is not null)
                yield return new(registration.Key, form);
            else
                throw new InvalidOperationException($"Unable to build form {registration.Value.Name}");
        }
    }

    public IFormSetupOptions AddRepository<TRepository>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        var repoInterfaces = typeof(TRepository)
            .GetInterfaces()
            .Where(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRepository<>))
            .ToList();

        if (repoInterfaces.Count == 0)
            throw new InvalidOperationException(
                $"{typeof(TRepository).Name} does not implement IRepository<T>");


        foreach (Type repoInterface in repoInterfaces)
        {
            CheckAlreadyRegistered(services, repoInterface);
            services.Add(new ServiceDescriptor(repoInterface, typeof(TRepository), lifetime));
        }
        return this;
    }

    private static void CheckAlreadyRegistered(IServiceCollection services, Type repoInterface)
    {
        ServiceDescriptor? existing = services.FirstOrDefault(s => s.ServiceType == repoInterface);

        if (existing == null)
            return;

        throw new InvalidOperationException(
            $"Repository for {repoInterface} already registered: " +
            $"{existing.ImplementationType?.Name}");
    }
}
