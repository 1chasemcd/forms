using System.Reflection;
using System.Text.Json;
using FormsApi.Form.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.Recalculate;

[Route("api/[controller]")]
[ApiController]
public class RecalculateEventController(IServiceProvider serviceProvider) : ControllerBase
{
    [HttpPost("{serviceType}/{method}")]
    public RecalculateEventResult PerformAction(
        [FromRoute] SerializedType serviceType,
        [FromRoute] string method,
        [FromBody] JsonElement body)
    {
        object? serviceInstance = serviceProvider.GetService(serviceType.GetRuntimeType()) ?? throw new InvalidOperationException($"Service '{serviceType}' is not registered.");
        IEnumerable<MethodInfo> methodInfos = serviceType
            .GetRuntimeType()
            .GetMethods()
            .Where(m =>
                m.Name.Equals(method, StringComparison.OrdinalIgnoreCase) &&
                m.ReturnType == typeof(RecalculateEventResult))
            .ToList();


        if (!methodInfos.Any())
            throw new InvalidOperationException($"Method '{method}' not found on service '{serviceType}'.");
        if (methodInfos.Count() > 1)
            throw new InvalidOperationException($"Multiple overloads for '{method}' found on service '{serviceType}'.");

        MethodInfo methodToUse = methodInfos.Single();

        ParameterInfo[] parameters = methodToUse.GetParameters();
        if (parameters.Length > 1)
            throw new InvalidOperationException($"Multiple overloads for '{method}' found on service '{serviceType}'.");

        object?[] args = new object?[parameters.Length];
        if (parameters.SingleOrDefault() is { } parameter)
            args[0] = body.Deserialize(parameter.ParameterType);

        object? result = methodToUse.Invoke(serviceInstance, args);


        return (RecalculateEventResult)result!;
    }
}
