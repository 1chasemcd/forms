using System;
using FormsApi.Definition.InputMetadata;

namespace FormsApi.Builder.Metadata;

public class TextAreaInputMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<TextAreaInputMetadataBuilder<TModel>, TModel>,
    ILabelable<TextAreaInputMetadataBuilder<TModel>, TModel>,
    IMaxLengthable<TextAreaInputMetadataBuilder<TModel>, TModel>,
    IRecalculatable<TextAreaInputMetadataBuilder<TModel>, TModel>,
    IRequirable<TextAreaInputMetadataBuilder<TModel>, TModel>,
    IVisible<TextAreaInputMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public PropertyOrConstant<TModel, int>? MaxLength { get; set; }
    public IRecalculateEvent<TModel>? RecalculateEvent { get; set; }
    public PropertyOrConstant<TModel, bool>? Required { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public InputType GetInputType() => InputType.TextArea;
}
