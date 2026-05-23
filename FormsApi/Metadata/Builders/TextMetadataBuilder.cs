using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class TextMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<TextMetadataBuilder<TModel>, TModel>,
    ILabelable<TextMetadataBuilder<TModel>, TModel>,
    IMaxLengthable<TextMetadataBuilder<TModel>, TModel>,
    IRecalculatable<TextMetadataBuilder<TModel>, TModel>,
    IRequirable<TextMetadataBuilder<TModel>, TModel>,
    IVisible<TextMetadataBuilder<TModel>, TModel>

{
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, string?>? Label { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? MaxLength { get; set; }
    public IFormServiceMethodBuilder<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.Text;
}
