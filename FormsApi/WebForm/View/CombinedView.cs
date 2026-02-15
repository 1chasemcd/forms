using System;

namespace FormsApi.WebForm.View;

public sealed class CombinedView : View
{
    public required IEnumerable<View> Views { get; init; }
}
