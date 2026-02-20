namespace FormsApi.Common.Types;

public sealed record Currency
{
    private readonly decimal _value;
    public static implicit operator decimal(Currency value) => value._value;
    public static implicit operator Currency(decimal value) => new(value);
    private Currency(decimal value) => _value = value;
}
