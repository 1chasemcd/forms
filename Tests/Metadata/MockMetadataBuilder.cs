using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata.Builders;
using FormsApi.Metadata.Interfaces;

namespace Tests.Metadata;

public class MockMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<MockMetadataBuilder<TModel>, TModel>,
    ILabelable<MockMetadataBuilder<TModel>, TModel>,
    IMaxLengthable<MockMetadataBuilder<TModel>, TModel>,
    IPrecisionAndScalable<MockMetadataBuilder<TModel>, TModel>,
    IRecalculatable<MockMetadataBuilder<TModel>, TModel>,
    IRequirable<MockMetadataBuilder<TModel>, TModel>,
    IValueRangable<MockMetadataBuilder<TModel>, TModel, int>,
    IVisible<MockMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public PropertyOrConstant<TModel, int>? MaxLength { get; set; }
    public IRecalculateEvent<TModel>? RecalculateEvent { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, int>? MinValue { get; set; }
    public PropertyOrConstant<TModel, int>? MaxValue { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public PropertyOrConstant<TModel, int>? Precision { get; set; }
    public PropertyOrConstant<TModel, int>? Scale { get; set; }
    public InputType GetInputType() => InputType.Text;
}
