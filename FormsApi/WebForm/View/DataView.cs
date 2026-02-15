using System;
using FormsApi.WebForm.Field;

namespace FormsApi.WebForm.View;

public sealed class DataView : View
{
    public required IEnumerable<BaseField> Fields { get; init; }
}
