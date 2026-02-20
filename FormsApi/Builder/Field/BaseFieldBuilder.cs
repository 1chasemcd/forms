
using FormsApi.Form;
using FormsApi.Form.Field;

namespace FormsApi.Builder.Field;

public abstract class BaseFieldBuilder<TModel>
{
    public PropertyOrConstantBuilder<TModel, string>? Label { get; set; }
    public IEnumerable<ModelMemberBuilder<TModel, object>>? PropertiesToUpdateOnChange { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Hidden { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Disabled { get; set; }
    public FormElementSize Width { get; set; }
    internal BaseField Build()
    {
        BaseField field = BuildImpl();

        return field with
        {
            Label = Label?.Build(),
            PropertiesToUpdateOnChange = PropertiesToUpdateOnChange?.Select(x => x.Build()),
            Hidden = Hidden?.Build(),
            Disabled = Disabled?.Build(),
            Width = Width
        };
    }
    protected abstract BaseField BuildImpl();
}
