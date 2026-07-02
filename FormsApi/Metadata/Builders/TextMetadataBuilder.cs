using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class TextMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<TextMetadataBuilder<TModel>, TModel>,
    ILabelable<TextMetadataBuilder<TModel>, TModel>,
    IMaxLengthable<TextMetadataBuilder<TModel>, TModel>,
    IServiceMethodCaller<TextMetadataBuilder<TModel>, TModel>,
    IRequirable<TextMetadataBuilder<TModel>, TModel>,
    IVisible<TextMetadataBuilder<TModel>, TModel>

{
    public FormValueRefBuilder<TModel, bool>? Enabled { get; set; }
    public FormValueRefBuilder<TModel, string?>? Label { get; set; }
    public FormValueRefBuilder<TModel, int>? MaxLength { get; set; }
    public IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
    public FormValueRefBuilder<TModel, bool>? Required { get; set; }
    public FormValueRefBuilder<TModel, bool>? Visible { get; set; }
    public FieldType GetFieldType() => FieldType.Text;
}
