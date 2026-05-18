using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
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
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IFormServiceMethod<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, decimal>? MinValue { get; set; }
    public PropertyOrConstant<TModel, decimal>? MaxValue { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.Currency;
}
