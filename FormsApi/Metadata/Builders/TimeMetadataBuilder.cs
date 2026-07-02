using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class TimeMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<DateMetadataBuilder<TModel>, TModel>,
    ILabelable<DateMetadataBuilder<TModel>, TModel>,
    IServiceMethodCaller<DateMetadataBuilder<TModel>, TModel>,
    IRequirable<DateMetadataBuilder<TModel>, TModel>,
    IValueRangable<DateMetadataBuilder<TModel>, TModel, TimeOnly>,
    IVisible<DateMetadataBuilder<TModel>, TModel>
{
    public FormValueRefBuilder<TModel, bool>? Enabled { get; set; }
    public FormValueRefBuilder<TModel, string?>? Label { get; set; }
    public IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
    public FormValueRefBuilder<TModel, bool>? Required { get; set; }
    public FormValueRefBuilder<TModel, TimeOnly>? MinValue { get; set; }
    public FormValueRefBuilder<TModel, TimeOnly>? MaxValue { get; set; }
    public FormValueRefBuilder<TModel, bool>? Visible { get; set; }
    public FieldType GetFieldType() => FieldType.Time;
}
