using System.Linq.Expressions;
using FormsApi.Common.Types;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public sealed class StaticTextBuilder<TModel>
    : BaseFieldBuilder<TModel, StaticTextBuilder<TModel>>
{
    private PropertyOrConstantBuilder<TModel, StaticText>? _builder;
    public StaticTextBuilder(Expression<Func<TModel, StaticText>> propertyBuilder)
    {
        _builder = new PropertyOrConstantBuilder<TModel, StaticText>(propertyBuilder);
    }

    public StaticTextBuilder(string text)
    {
        Label = new PropertyOrConstantBuilder<TModel, string>(text);
    }

    protected override StaticTextField BuildField()
    {
        return new StaticTextField()
        {
            Label = _builder?.Build()
        };
    }
}

