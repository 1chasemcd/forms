using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
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
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public PropertyOrConstant<TModel, int>? MaxLength { get; set; }
    public IFormServiceMethod<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.Text;
}
