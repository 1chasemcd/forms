using System.Linq.Expressions;
using FormsApi.Common;
using FormsApi.Form.Field;
using FormsApi.Form.Primitives;
using FormsApi.FormAction;

namespace FormsApi.Builder.Field;

public static class Button
{
    public static ButtonBuilder<TModel> Build<TModel>(TModel model)
    {
        return new ButtonBuilder<TModel>();
    }
}

public sealed class ButtonBuilder<TModel>
    : BaseFieldBuilder<TModel, ButtonBuilder<TModel>>
{
    public IEnumerable<ModelMemberBuilder<TModel, object>>? PropsToUpdate { get; set; }
    public IFormActionBuilder<TModel>? FormActionBuilder { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Disabled { get; set; }
    protected override ButtonField BuildField()
    {
        return new ButtonField()
        {
            OnChange = BuildOnChangeEvent()
        };
    }

    private OnChangeEvent BuildOnChangeEvent()
    {
        return new OnChangeEvent()
        {
            PropertiesToUpdate = PropsToUpdate?.Select(p => p.Build()),
            FormAction = FormActionBuilder?.Build()
        };
    }
    protected override string GetDefaultLabel() => FormActionBuilder?.Build().Method.CamelCaseToWords() ?? "Button";

    public ButtonBuilder<TModel> WithPropsToUpdate(params Expression<Func<TModel, object>>[] props)
    {
        PropsToUpdate = props.Select(x => new ModelMemberBuilder<TModel, object>(x));
        return this;
    }

    public ButtonBuilder<TModel> WithActionOnChange<TService>(Expression<Func<TService, Func<TModel, FormActionResult>>> serviceMethod)
    {
        FormActionBuilder = new FormActionBuilder<TService, TModel>(serviceMethod);
        return this;
    }

    public ButtonBuilder<TModel> WithActionOnChange<TService>(Expression<Func<TService, Func<FormActionResult>>> serviceMethod)
    {
        FormActionBuilder = new FormActionBuilder<TService, TModel>(serviceMethod);
        return this;
    }
}
