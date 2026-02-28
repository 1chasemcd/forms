using FormsApi.Repository;
using Microsoft.Extensions.Logging;

namespace FormsApi.Common.Registry;

public class RepositoryRegistry(
    ILogger<RepositoryRegistry> logger)
    : BaseRegistry<Type, IRepository<object>>(logger);
