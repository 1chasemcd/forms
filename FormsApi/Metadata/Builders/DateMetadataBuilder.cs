using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
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
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IFormServiceMethod<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, DateOnly>? MinValue { get; set; }
    public PropertyOrConstant<TModel, DateOnly>? MaxValue { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.Date;
}
