using System;

namespace FormsApi.WebForm.Field;

public class StaticText : BaseField
{
    public required PropertyOrConstant<string> Text { get; set; }
}
