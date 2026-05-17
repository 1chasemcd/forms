using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class CurrencyInputMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<CurrencyInputMetadataBuilder<TModel>, TModel>,
    ILabelable<CurrencyInputMetadataBuilder<TModel>, TModel>,
    IRecalculatable<CurrencyInputMetadataBuilder<TModel>, TModel>,
    IRequirable<CurrencyInputMetadataBuilder<TModel>, TModel>,
    IValueRangable<CurrencyInputMetadataBuilder<TModel>, TModel, decimal>,
    IVisible<CurrencyInputMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IRecalculateEvent<TModel>? RecalculateEvent { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, decimal>? MinValue { get; set; }
    public PropertyOrConstant<TModel, decimal>? MaxValue { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public InputType GetInputType() => InputType.Currency;
}
