using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class LabelValueMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    ILabelable<LabelValueMetadataBuilder<TModel>, TModel>,
    IVisible<LabelValueMetadataBuilder<TModel>, TModel>
{
    public FormValueRefBuilder<TModel, string?>? Label { get; set; }
    public FormValueRefBuilder<TModel, bool>? Visible { get; set; }
    public FieldType GetFieldType() => FieldType.LabelValue;
}
