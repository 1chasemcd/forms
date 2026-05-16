using FormsApi.Definition.InputMetadata;

namespace FormsApi.Builder.Metadata;

public class CheckBoxInputMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<CheckBoxInputMetadataBuilder<TModel>, TModel>,
    ILabelable<CheckBoxInputMetadataBuilder<TModel>, TModel>,
    IRecalculatable<CheckBoxInputMetadataBuilder<TModel>, TModel>,
    IRequirable<CheckBoxInputMetadataBuilder<TModel>, TModel>,
    IVisible<CheckBoxInputMetadataBuilder<TModel>, TModel>

{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IRecalculateEvent<TModel>? RecalculateEvent { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public InputType GetInputType() => InputType.CheckBox;
}
