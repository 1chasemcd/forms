using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class StaticTextBuilder<TModel>(string text)
    : BaseFieldBuilder<TModel, StaticTextBuilder<TModel>>
{
    protected override StaticTextField BuildImpl()
    {
        return new StaticTextField()
        {
            Text = text
        };
    }
}

