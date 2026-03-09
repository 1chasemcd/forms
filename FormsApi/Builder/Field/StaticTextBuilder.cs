using FormsApi.Form.Field;
using FormsApi.Form.Primitives;

namespace FormsApi.Builder.Field;

public sealed class StaticTextBuilder<TModel>(string text)
    : BaseFieldBuilder<TModel, StaticTextBuilder<TModel>>
{
    protected override StaticTextField BuildField()
    {
        return new StaticTextField()
        {
            Text = new Constant(text)
        };
    }
}

