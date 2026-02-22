namespace FormsApi.Form.Primitives;

public abstract record PropertyOrConstant;
public sealed record Property(string Value) : PropertyOrConstant;
public sealed record Constant(object Value) : PropertyOrConstant;
