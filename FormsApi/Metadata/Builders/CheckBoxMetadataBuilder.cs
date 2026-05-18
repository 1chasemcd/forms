using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class CheckBoxMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<CheckBoxMetadataBuilder<TModel>, TModel>,
    ILabelable<CheckBoxMetadataBuilder<TModel>, TModel>,
    IRecalculatable<CheckBoxMetadataBuilder<TModel>, TModel>,
    IRequirable<CheckBoxMetadataBuilder<TModel>, TModel>,
    IVisible<CheckBoxMetadataBuilder<TModel>, TModel>

{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IFormServiceMethod<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.CheckBox;
}
