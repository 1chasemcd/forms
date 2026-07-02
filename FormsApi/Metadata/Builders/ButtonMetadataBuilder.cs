using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class ButtonMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<ButtonMetadataBuilder<TModel>, TModel>,
    ILabelable<ButtonMetadataBuilder<TModel>, TModel>,
    IServiceMethodCaller<ButtonMetadataBuilder<TModel>, TModel>,
    IVisible<ButtonMetadataBuilder<TModel>, TModel>
{
    public FormValueRefBuilder<TModel, bool>? Enabled { get; set; }
    public FormValueRefBuilder<TModel, string?>? Label { get; set; }
    public IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
    public FormValueRefBuilder<TModel, bool>? Visible { get; set; }
    public FieldType GetFieldType() => FieldType.Button;
}
