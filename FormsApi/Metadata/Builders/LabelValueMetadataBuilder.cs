using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class LabelValueMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    ILabelable<LabelValueMetadataBuilder<TModel>, TModel>,
    IVisible<LabelValueMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.LabelValue;
}
