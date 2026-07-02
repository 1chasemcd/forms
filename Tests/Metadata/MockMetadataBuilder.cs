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
    public FormValueRefBuilder<TModel, bool>? Enabled { get; set; }
    public FormValueRefBuilder<TModel, string?>? Label { get; set; }
    public FormValueRefBuilder<TModel, int>? MaxLength { get; set; }
    public IServiceMethodBuilder<TModel>? ServiceMethod { get; set; }
    public FormValueRefBuilder<TModel, bool>? Required { get; set; }
    public FormValueRefBuilder<TModel, int>? MinValue { get; set; }
    public FormValueRefBuilder<TModel, int>? MaxValue { get; set; }
    public FormValueRefBuilder<TModel, bool>? Visible { get; set; }
    public FormValueRefBuilder<TModel, int>? Precision { get; set; }
    public FormValueRefBuilder<TModel, int>? Scale { get; set; }
    public FieldType GetFieldType() => FieldType.Text;
}
