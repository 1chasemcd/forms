using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class CurrencyMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<CurrencyMetadataBuilder<TModel>, TModel>,
    ILabelable<CurrencyMetadataBuilder<TModel>, TModel>,
    IRecalculatable<CurrencyMetadataBuilder<TModel>, TModel>,
    IRequirable<CurrencyMetadataBuilder<TModel>, TModel>,
    IValueRangable<CurrencyMetadataBuilder<TModel>, TModel, decimal>,
    IVisible<CurrencyMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, string?>? Label { get; set; }
    public IFormServiceMethodBuilder<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, decimal>? MinValue { get; set; }
    public PropertyOrConstantBuilder<TModel, decimal>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.Currency;
}
