using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class ButtonMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<ButtonMetadataBuilder<TModel>, TModel>,
    ILabelable<ButtonMetadataBuilder<TModel>, TModel>,
    IRecalculatable<ButtonMetadataBuilder<TModel>, TModel>,
    IVisible<ButtonMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, string?>? Label { get; set; }
    public IFormServiceMethodBuilder<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.Button;
}
