using FormsApi.Common;
using FormsApi.Contract;

namespace Tests;

public static class TestHelperExensions
{
    public static object? InnerValue(this IFormValueRefBuilder? valueRef)
    {
        if (valueRef is null) return null;
        FormValueRef build = valueRef.Build();
        return build is ModelValue p ? p.Value : ((ConstantValue)build).Value;
    }

    public static object? InnerValue(this FormValueRef? valueRef)
    {
        if (valueRef is null) return null;
        return valueRef is ModelValue p ? p.Value : ((ConstantValue)valueRef).Value;
    }
}
