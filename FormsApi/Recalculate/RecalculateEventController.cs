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
    public ActionResult<RecalculateEventResult<object>> PerformAction(
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
                m.ReturnType.IsGenericType &&
                m.ReturnType.GetGenericTypeDefinition() == typeof(RecalculateEventResult<>))
            .ToList();


        if (!methodInfos.Any())
            throw new InvalidOperationException($"Method '{method}' not found on service '{serviceType}'.");
        if (methodInfos.Count() > 1)
            throw new InvalidOperationException($"Multiple overloads for '{method}' found on service '{serviceType}'.");

        MethodInfo methodToUse = methodInfos.Single();

        ParameterInfo[] parameters = methodToUse.GetParameters();
        if (parameters.Length > 1)
            throw new InvalidOperationException($"Expected method with 1 parameter but was {parameters.Length} for {serviceType}.{method}");

        object?[] args = new object?[parameters.Length];
        if (parameters.SingleOrDefault() is { } parameter)
            args[0] = body.Deserialize(parameter.ParameterType);

        object? result = methodToUse.Invoke(serviceInstance, args) ?? throw new InvalidOperationException("Method result was null.");
        object? model = result.GetType().GetProperty(nameof(RecalculateEventResult<>.Model))?.GetValue(result);
        return Ok(new RecalculateEventResult<object>
        {
            Model = model
        });
    }
}
