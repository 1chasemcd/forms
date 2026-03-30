using System.Text.Json.Serialization;
using FormsApi.Json;

namespace FormsApi.Common.Types;

[JsonConverter(typeof(CurrencyJsonConverter))]
public readonly struct Currency
{
    public Currency() { _value = 0; }
    private readonly decimal _value;
    public static implicit operator decimal(Currency value) => value._value;
    public static implicit operator Currency(decimal value) => new(value);
    private Currency(decimal value) => _value = value;
}
