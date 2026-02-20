using System;
using FormsApi.Common.Types;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public class StaticTextBuilder<TModel>(string text)
    : BaseFieldBuilder<TModel>
{
    protected override StaticTextField BuildImpl()
    {
        return new StaticTextField()
        {
            Text = text
        };
    }
}

