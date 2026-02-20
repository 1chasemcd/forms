namespace FormsApi.Form;

public abstract record PropertyOrConstant<T>;
public sealed record Property<T>(string PropertyName) : PropertyOrConstant<T>;
public sealed record Constant<T>(T Value) : PropertyOrConstant<T>;
