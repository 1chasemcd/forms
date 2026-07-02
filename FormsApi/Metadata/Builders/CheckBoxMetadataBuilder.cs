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
    public FormValueRefBuilder<TModel, bool>? Enabled { get; set; }
    public FormValueRefBuilder<TModel, string?>? Label { get; set; }
    public IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
    public FormValueRefBuilder<TModel, bool>? Required { get; set; }
    public FormValueRefBuilder<TModel, bool>? Visible { get; set; }
    public FieldType GetFieldType() => FieldType.CheckBox;
}
