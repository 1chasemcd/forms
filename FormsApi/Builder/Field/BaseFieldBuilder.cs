
using FormsApi.Common;
using FormsApi.Definition.Field;
using FormsApi.Definition.Metadata;
using FormsApi.Definition.Primitives;

namespace FormsApi.Builder.Field;

public interface IFieldBuilder<TModel> : IBuildable<FieldDefinition>;
public abstract class BaseFieldBuilder<TModel, TField> : IFieldBuilder<TModel>
{
    public required ModelMemberBuilder<TModel, TField> Property { get; set; }
    public abstract FieldType Type { get; }
    public PropertyOrConstantBuilder<TModel, string>? Label { get; set; }
    public PropertyOrConstantBuilder<TModel, bool>? Visible { get; set; }
    public int? Width { get; set; }
    private IList<MetadataDefinition> _metadatas = [];
    public FieldDefinition Build()
    {
        _metadatas = [];

        AddMetadatas();

        return new()
        {
            Property = Property.Build(),
            Type = Type,
            FieldMetadatas = _metadatas.Count == 0 ? null : _metadatas,
        };
    }

    private void AddMetadatas()
    {
        AddMetadata(MetadataType.Label, Label);
        AddMetadata(MetadataType.Visible, Visible);
        AddMetadata(MetadataType.Width, Width);

        if (this is IEnablable<TModel> enablable)
            AddMetadata(MetadataType.Enabled, enablable.Enabled);

        if (this is IMaxLengthable<TModel> maxLengthable)
            AddMetadata(MetadataType.MaxLength, maxLengthable.MaxLength);

        if (this is IPrecisionAndScalable<TModel> psable)
        {
            AddMetadata(MetadataType.Precision, psable.Precision);
            AddMetadata(MetadataType.Scale, psable.Scale);
        }

        if (this is IRecalculatable<TModel> recalculatable)
            AddMetadata(MetadataType.RecalculateEvent, recalculatable.RecalculateEvent);

        if (this is IRequirable<TModel> requirable)
            AddMetadata(MetadataType.Required, requirable.Required);

        if (this is IValueRangable<TModel, TField> rangable)
        {
            AddMetadata(MetadataType.MinValue, rangable.MinValue);
            AddMetadata(MetadataType.MaxValue, rangable.MaxValue);
        }
    }

    private void AddMetadata<T>(MetadataType type, IBuildable<T>? builder)
    {
        if (builder == null || builder.Build() is not { } value) return;
        _metadatas.Add(new MetadataDefinition
        {
            Type = type,
            Value = value
        });
    }

    private void AddMetadata(MetadataType type, object? value)
    {
        if (value == null) return;
        _metadatas.Add(new MetadataDefinition
        {
            Type = type,
            Value = value
        });
    }
}
