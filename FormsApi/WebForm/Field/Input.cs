using System;

namespace FormsApi.WebForm.Field;

public class Input : BaseField
{
    public required string Property { get; init; }
}
