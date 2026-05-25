using System.Reflection;
using System.Text.Json;
using FormsApi.Contract;
using FormsApi.Contract.PostRequest;
using Microsoft.AspNetCore.Mvc;

namespace FormsApi.ServiceMethod;

[Route("api/[controller]")]
[ApiController]
public sealed class ServiceMethodController(IServiceProvider serviceProvider) : ControllerBase
{
    [HttpPost("{serviceType}/{method}")]
    public ActionResult<ServiceMethodResponse> RunMethod(
        [FromRoute] TypeDto serviceType,
        [FromRoute] string method,
        [FromBody] JsonElement body)
    {
        object? serviceInstance = serviceProvider.GetService(serviceType.GetRuntimeType()) ?? throw new InvalidOperationException($"Service '{serviceType}' is not registered.");
        IEnumerable<MethodInfo> methodInfos = serviceType
            .GetRuntimeType()
            .GetMethods()
            .Where(m =>
                m.Name.Equals(method, StringComparison.OrdinalIgnoreCase) &&
                (m.ReturnType == typeof(PostRequestAction) ||
                Nullable.GetUnderlyingType(m.ReturnType) == typeof(PostRequestAction)))
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

        object? result = methodToUse.Invoke(serviceInstance, args);
        object? model = args.Length > 0 ? args[0] : null;
        return Ok(new ServiceMethodResponse
        {
            Model = JsonSerializer.SerializeToElement(model),
            PostAction = result as PostRequestAction
        });
    }
}
