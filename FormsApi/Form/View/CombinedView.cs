namespace FormsApi.Form.View;

public sealed record class CombinedView : BaseView
{
    public required IEnumerable<BaseView> Views { get; init; }
}
