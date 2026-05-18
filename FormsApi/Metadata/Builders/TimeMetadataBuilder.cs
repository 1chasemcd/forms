using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Builders;

public class TimeMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<DateMetadataBuilder<TModel>, TModel>,
    ILabelable<DateMetadataBuilder<TModel>, TModel>,
    IRecalculatable<DateMetadataBuilder<TModel>, TModel>,
    IRequirable<DateMetadataBuilder<TModel>, TModel>,
    IValueRangable<DateMetadataBuilder<TModel>, TModel, TimeOnly>,
    IVisible<DateMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IFormServiceMethod<TModel>? FormServiceMethod { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, TimeOnly>? MinValue { get; set; }
    public PropertyOrConstant<TModel, TimeOnly>? MaxValue { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public ControlType GetControlType() => ControlType.Time;
}
