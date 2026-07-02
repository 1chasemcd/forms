using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class CurrencyMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<CurrencyMetadataBuilder<TModel>, TModel>,
    ILabelable<CurrencyMetadataBuilder<TModel>, TModel>,
    IServiceMethodCaller<CurrencyMetadataBuilder<TModel>, TModel>,
    IRequirable<CurrencyMetadataBuilder<TModel>, TModel>,
    IValueRangable<CurrencyMetadataBuilder<TModel>, TModel, decimal>,
    IVisible<CurrencyMetadataBuilder<TModel>, TModel>
{
    public FormValueRefBuilder<TModel, bool>? Enabled { get; set; }
    public FormValueRefBuilder<TModel, string?>? Label { get; set; }
    public IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
    public FormValueRefBuilder<TModel, bool>? Required { get; set; }
    public FormValueRefBuilder<TModel, decimal>? MinValue { get; set; }
    public FormValueRefBuilder<TModel, decimal>? MaxValue { get; set; }
    public FormValueRefBuilder<TModel, bool>? Visible { get; set; }
    public FieldType GetFieldType() => FieldType.Currency;
}
