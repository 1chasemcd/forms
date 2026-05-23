using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class DateMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<DateMetadataBuilder<TModel>, TModel>,
    ILabelable<DateMetadataBuilder<TModel>, TModel>,
    IRecalculatable<DateMetadataBuilder<TModel>, TModel>,
    IRequirable<DateMetadataBuilder<TModel>, TModel>,
    IValueRangable<DateMetadataBuilder<TModel>, TModel, DateOnly>,
    IVisible<DateMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, string?>? Label { get; set; }
    public IFormServiceMethodBuilder<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, DateOnly>? MinValue { get; set; }
    public PropertyOrConstantBuilder<TModel, DateOnly>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.Date;
}
