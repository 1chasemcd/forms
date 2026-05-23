using FormsApi.Common;
using FormsApi.Contract;

namespace Tests;

public static class TestHelperExensions
{
    public static object? InnerValue(this IPropertyOrConstantBuilder? poc)
    {
        if (poc is null) return null;
        PropertyOrConstant build = poc.Build();
        return build is Property p ? p.Value : ((Constant)build).Value;
    }

    public static object? InnerValue(this PropertyOrConstant? poc)
    {
        if (poc is null) return null;
        return poc is Property p ? p.Value : ((Constant)poc).Value;
    }
}
