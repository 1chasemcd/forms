using System;

namespace FormsApi.WebForm;

public abstract class PropertyOrConstant<T> { }

public class PropertyValue<T> : PropertyOrConstant<T>
{
    public required string PropertyName { get; init; }
}

public class ConstantValue<T> : PropertyOrConstant<T>
{
    public required T Value { get; init; }
}
