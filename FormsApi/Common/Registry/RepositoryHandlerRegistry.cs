using System.Collections.Concurrent;
using FormsApi.Repository;
using Microsoft.Extensions.Logging;

namespace FormsApi.Common.Registry;

public class RepositoryHandlerRegistry(
    ILogger<RepositoryHandlerRegistry> logger)
    : BaseRegistry<Type, IRepositoryHandler<object>>(logger);
