using System;
using FormsApi.Definition.InputMetadata;

namespace FormsApi.Builder.Metadata;

public class ButtonInputMetadataBuilder<TModel> :
    IMetadataBuilder<TModel>,
    IEnablable<ButtonInputMetadataBuilder<TModel>, TModel>,
    ILabelable<ButtonInputMetadataBuilder<TModel>, TModel>,
    IRecalculatable<ButtonInputMetadataBuilder<TModel>, TModel>,
    IVisible<ButtonInputMetadataBuilder<TModel>, TModel>
{
    public PropertyOrConstant<TModel, bool>? Enabled { get; set; }
    public PropertyOrConstant<TModel, string?>? Label { get; set; }
    public IRecalculateEvent<TModel>? RecalculateEvent { get; set; }
    public PropertyOrConstant<TModel, bool>? Visible { get; set; }
    public InputType GetInputType() => InputType.Button;
}
