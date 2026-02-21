using System;
using FormsApi.Common.Types;

namespace Sample;

public class TestModel
{
    public string TextField { get; set; } = string.Empty;
    public DateOnly DateField { get; set; }
    public decimal NumericField { get; set; }
    public Currency CurrencyField { get; set; } = 0;
}
