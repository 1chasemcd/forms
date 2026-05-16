using System;
using FormsApi.Definition.InputMetadata;

namespace FormsApi.Builder.Metadata;

public class LabelValueMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    ILabelable<LabelValueMetadataBuilder<TModel>, TModel>,
    IVisible<LabelValueMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public InputType GetInputType() => InputType.LabelValue;
}
