using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class TextAreaMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<TextAreaMetadataBuilder<TModel>, TModel>,
    ILabelable<TextAreaMetadataBuilder<TModel>, TModel>,
    IMaxLengthable<TextAreaMetadataBuilder<TModel>, TModel>,
    IRecalculatable<TextAreaMetadataBuilder<TModel>, TModel>,
    IRequirable<TextAreaMetadataBuilder<TModel>, TModel>,
    IVisible<TextAreaMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public PropertyOrConstant<TModel, int>? MaxLength { get; set; }
    public IFormServiceMethod<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.TextArea;
}
