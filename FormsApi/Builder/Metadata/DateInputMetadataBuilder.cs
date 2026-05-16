using System;
using FormsApi.Definition.InputMetadata;

namespace FormsApi.Builder.Metadata;

public class DateInputMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<DateInputMetadataBuilder<TModel>, TModel>,
    ILabelable<DateInputMetadataBuilder<TModel>, TModel>,
    IRecalculatable<DateInputMetadataBuilder<TModel>, TModel>,
    IRequirable<DateInputMetadataBuilder<TModel>, TModel>,
    IValueRangable<DateInputMetadataBuilder<TModel>, TModel, DateOnly>,
    IVisible<DateInputMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IRecalculateEvent<TModel>? RecalculateEvent { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, DateOnly>? MinValue { get; set; }
    public PropertyOrConstant<TModel, DateOnly>? MaxValue { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public InputType GetInputType() => InputType.Date;
}
