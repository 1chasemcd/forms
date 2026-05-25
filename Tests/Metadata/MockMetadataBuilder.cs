using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Builders;
using FormsApi.Metadata.Interfaces;

namespace Tests.Metadata;

public class MockMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<MockMetadataBuilder<TModel>, TModel>,
    ILabelable<MockMetadataBuilder<TModel>, TModel>,
    IMaxLengthable<MockMetadataBuilder<TModel>, TModel>,
    IPrecisionAndScalable<MockMetadataBuilder<TModel>, TModel>,
    IServiceMethodCaller<MockMetadataBuilder<TModel>, TModel>,
    IRequirable<MockMetadataBuilder<TModel>, TModel>,
    IValueRangable<MockMetadataBuilder<TModel>, TModel, int>,
    IVisible<MockMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstantBuilder<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstantBuilder<TModel, string?>? Label { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? MaxLength { get; set; }
    public IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Required { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? MinValue { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? MaxValue { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Visible { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? Precision { get; set; }
    public PropertyOrConstantBuilder<TModel, int>? Scale { get; set; }
    public ControlType GetControlType() => ControlType.Text;
}
