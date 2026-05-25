using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class TextAreaMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<TextAreaMetadataBuilder<TModel>, TModel>,
    ILabelable<TextAreaMetadataBuilder<TModel>, TModel>,
    IMaxLengthable<TextAreaMetadataBuilder<TModel>, TModel>,
    IServiceMethodCaller<TextAreaMetadataBuilder<TModel>, TModel>,
    IRequirable<TextAreaMetadataBuilder<TModel>, TModel>,
    IVisible<TextAreaMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, string?>? Label { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? MaxLength { get; set; }
    public IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.TextArea;
}
