using System.Numerics;
using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class NumericMetadataBuilder<TModel, TNumber> :
    IMetadataBuilder<TModel>,
    IEnablable<NumericMetadataBuilder<TModel, TNumber>, TModel>,
    ILabelable<NumericMetadataBuilder<TModel, TNumber>, TModel>,
    IPrecisionAndScalable<NumericMetadataBuilder<TModel, TNumber>, TModel>,
    IRecalculatable<NumericMetadataBuilder<TModel, TNumber>, TModel>,
    IRequirable<NumericMetadataBuilder<TModel, TNumber>, TModel>,
    IValueRangable<NumericMetadataBuilder<TModel, TNumber>, TModel, TNumber>,
    IVisible<NumericMetadataBuilder<TModel, TNumber>, TModel>
    where TNumber : INumber<TNumber>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IFormServiceMethod<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, TNumber>? MinValue { get; set; }
    public PropertyOrConstant<TModel, TNumber>? MaxValue { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public PropertyOrConstant<TModel, int>? Precision { get; set; }
    public PropertyOrConstant<TModel, int>? Scale { get; set; }
    public ControlType GetControlType() => ControlType.Numeric;
}
