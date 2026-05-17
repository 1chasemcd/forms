using System.Numerics;
using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class NumericInputMetadataBuilder<TModel, TNumber> :
    IMetadataBuilder<TModel>,
    IEnablable<NumericInputMetadataBuilder<TModel, TNumber>, TModel>,
    ILabelable<NumericInputMetadataBuilder<TModel, TNumber>, TModel>,
    IPrecisionAndScalable<NumericInputMetadataBuilder<TModel, TNumber>, TModel>,
    IRecalculatable<NumericInputMetadataBuilder<TModel, TNumber>, TModel>,
    IRequirable<NumericInputMetadataBuilder<TModel, TNumber>, TModel>,
    IValueRangable<NumericInputMetadataBuilder<TModel, TNumber>, TModel, TNumber>,
    IVisible<NumericInputMetadataBuilder<TModel, TNumber>, TModel>
    where TNumber : INumber<TNumber>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IRecalculateEvent<TModel>? RecalculateEvent { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, TNumber>? MinValue { get; set; }
    public PropertyOrConstant<TModel, TNumber>? MaxValue { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public PropertyOrConstant<TModel, int>? Precision { get; set; }
    public PropertyOrConstant<TModel, int>? Scale { get; set; }
    public InputType GetInputType() => InputType.Numeric;
}
