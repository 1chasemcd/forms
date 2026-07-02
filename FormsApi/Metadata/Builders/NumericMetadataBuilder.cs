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
    IServiceMethodCaller<NumericMetadataBuilder<TModel, TNumber>, TModel>,
    IRequirable<NumericMetadataBuilder<TModel, TNumber>, TModel>,
    IValueRangable<NumericMetadataBuilder<TModel, TNumber>, TModel, TNumber>,
    IVisible<NumericMetadataBuilder<TModel, TNumber>, TModel>
    where TNumber : INumber<TNumber>
{
    public FormValueRefBuilder<TModel, bool>? Enabled { get; set; }
    public FormValueRefBuilder<TModel, string?>? Label { get; set; }
    public IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
    public FormValueRefBuilder<TModel, bool>? Required { get; set; }
    public FormValueRefBuilder<TModel, TNumber>? MinValue { get; set; }
    public FormValueRefBuilder<TModel, TNumber>? MaxValue { get; set; }
    public FormValueRefBuilder<TModel, bool>? Visible { get; set; }
    public FormValueRefBuilder<TModel, int>? Precision { get; set; }
    public FormValueRefBuilder<TModel, int>? Scale { get; set; }
    public FieldType GetFieldType() => FieldType.Numeric;
}
