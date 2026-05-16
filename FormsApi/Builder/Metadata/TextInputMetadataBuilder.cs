using FormsApi.Definition.InputMetadata;

namespace FormsApi.Builder.Metadata;

public class TextInputMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<TextInputMetadataBuilder<TModel>, TModel>,
    ILabelable<TextInputMetadataBuilder<TModel>, TModel>,
    IMaxLengthable<TextInputMetadataBuilder<TModel>, TModel>,
    IRecalculatable<TextInputMetadataBuilder<TModel>, TModel>,
    IRequirable<TextInputMetadataBuilder<TModel>, TModel>,
    IVisible<TextInputMetadataBuilder<TModel>, TModel>

{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public PropertyOrConstant<TModel, int>? MaxLength { get; set; }
    public IRecalculateEvent<TModel>? RecalculateEvent { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public InputType GetInputType() => InputType.Text;
}
