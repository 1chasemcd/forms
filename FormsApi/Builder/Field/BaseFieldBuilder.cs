
using FormsApi.Definition.Field;
using FormsApi.Definition.Metadata;
using FormsApi.Definition.Primitives;

namespace FormsApi.Builder.Field;

public abstract class BaseFieldBuilder<TModel>
{
    internal abstract FieldDefinition Build();
}
public abstract class BaseFieldBuilder<TModel, TField> : BaseFieldBuilder<TModel>
{
    public required ModelMemberBuilder<TModel, TField> Property { get; set; }
    public abstract FieldType Type { get; }
    public PropertyOrConstantBuilder<TModel, string>? Label { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Visible { get; set; }
    public FormElementSize? Width { get; set; }
    internal override FieldDefinition Build()
    {
        List<BaseMetadataDefinition> metadatas = [];

        if (Label != null)
            metadatas.Add(new LabelMetadata { Label = Label.Build() });

        if (Visible != null)
            metadatas.Add(new VisibleMetadata { Visible = Visible.Build() });

        if (Width != null)
            metadatas.Add(new WidthMetadata { Width = Width });

        if (this is IEnablable<TModel> enablable && enablable.Enabled != null)
            metadatas.Add(new EnabledMetadata { Enabled = enablable.Enabled.Build() });

        if (this is IMaxLengthable<TModel> maxLengthable && maxLengthable.MaxLength != null)
            metadatas.Add(new MaxLengthMetadata { MaxLength = maxLengthable.MaxLength.Build() });

        if (this is IPrecisionAndScalable<TModel> psable && (psable.Precision != null || psable.Scale != null))
            metadatas.Add(new PrecisionScaleMetadata
            {
                Precision = psable.Precision?.Build(),
                Scale = psable.Scale?.Build()
            });

        if (this is IRecalculatable<TModel> recalculatable && recalculatable.RecalculateEvent != null)
            metadatas.Add(new RecalculateEventMetadata { RecalculateEvent = recalculatable.RecalculateEvent.Build() });

        if (this is IRequirable<TModel> requirable && requirable.Required != null)
            metadatas.Add(new RequiredMetadata { Required = requirable.Required.Build() });

        if (this is IValueRangable<TModel, TField> rangable && (rangable.MaxValue != null || rangable.MinValue != null))
            metadatas.Add(new ValueRangeMetadata
            {
                MaxValue = rangable.MaxValue?.Build(),
                MinValue = rangable.MinValue?.Build()
            });

        return new()
        {
            Property = Property.Build(),
            Type = Type,
            FieldMetadatas = metadatas.Count == 0 ? null : metadatas,
        };
    }
}