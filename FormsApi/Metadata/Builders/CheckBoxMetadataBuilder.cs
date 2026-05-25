using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class CheckBoxMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<CheckBoxMetadataBuilder<TModel>, TModel>,
    ILabelable<CheckBoxMetadataBuilder<TModel>, TModel>,
    IServiceMethodCaller<CheckBoxMetadataBuilder<TModel>, TModel>,
    IRequirable<CheckBoxMetadataBuilder<TModel>, TModel>,
    IVisible<CheckBoxMetadataBuilder<TModel>, TModel>

{
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, string?>? Label { get; set; }
    public IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.CheckBox;
}
