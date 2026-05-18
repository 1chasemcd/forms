using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class ButtonMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<ButtonMetadataBuilder<TModel>, TModel>,
    ILabelable<ButtonMetadataBuilder<TModel>, TModel>,
    IRecalculatable<ButtonMetadataBuilder<TModel>, TModel>,
    IVisible<ButtonMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IFormServiceMethod<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.Button;
}
