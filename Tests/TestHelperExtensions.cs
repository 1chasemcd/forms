using FormsApi.Common;
using FormsApi.Contract;

namespace Tests;

public static class TestHelperExensions
{
    public static object? InnerValue(this IPropertyOrConstant? poc)
    {
        if (poc is null) return null;
        PropertyOrConstantDto build = poc.Build();
        return build is PropertyDto p ? p.Value : ((ConstantDto)build).Value;
    }

    public static object? InnerValue(this PropertyOrConstantDto? poc)
    {
        if (poc is null) return null;
        return poc is PropertyDto p ? p.Value : ((ConstantDto)poc).Value;
    }
}
