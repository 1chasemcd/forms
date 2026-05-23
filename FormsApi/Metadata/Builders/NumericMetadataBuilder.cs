using System.Numerics;
using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
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
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, string?>? Label { get; set; }
    public IFormServiceMethodBuilder<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, TNumber>? MinValue { get; set; }
    public PropertyOrConstantBuilder<TModel, TNumber>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Visible { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? Precision { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? Scale { get; set; }
    public ControlType GetControlType() => ControlType.Numeric;
}
