using System.Reflection;
using FormsApi.Builder;
using FormsApi.Builder.Metadata;
using FormsApi.Definition.InputMetadata;

namespace FormsApi.Definition.Service;

public sealed class MetadataProcessors
{
    public IEnumerable<Func<IMetadataBuilder<T>, IInputMetadataDto?>> GetProcessors<T>()
    {
        yield return propertyMetadata =>
            new InputTypeMetadataDto() { Value = propertyMetadata.GetInputType() };

        yield return propertyMetadata =>
            propertyMetadata is IEnablable<T> x && x.Enabled is not null
            ? new EnabledMetadataDto() { Value = x.Enabled.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is ILabelable<T> x && x.Label is not null
            ? new LabelMetadataDto() { Value = x.Label.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is IMaxLengthable<T> x && x.MaxLength is not null
            ? new MaxLengthMetadataDto() { Value = x.MaxLength.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is IPrecisionAndScalable<T> x && x.Precision is not null
            ? new PrecisionMetadataDto() { Value = x.Precision.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is IPrecisionAndScalable<T> x && x.Scale is not null
            ? new ScaleMetadataDto() { Value = x.Scale.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is IRecalculatable<T> x && x.RecalculateEvent is not null
            ? new RecalculateEventMetadataDto() { Value = x.RecalculateEvent.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is IRequirable<T> x && x.Required is not null
            ? new RequiredMetadataDto() { Value = x.Required.Build() }
            : null;

        yield return ProcessMaxValue;
        yield return ProcessMinValue;

        yield return propertyMetadata =>
            propertyMetadata is IVisible<T> x && x.Visible is not null
            ? new VisibleMetadataDto() { Value = x.Visible.Build() }
            : null;
    }

    private IInputMetadataDto? ProcessMaxValue<T>(IMetadataBuilder<T> propertyMetadata)
    {
        Type? rangeInterface = propertyMetadata.GetType().GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValueRangable<,,>));

        PropertyInfo? maxValueProperty = rangeInterface?.GetProperty(nameof(IValueRangable<,>.MaxValue));
        if (maxValueProperty?.GetValue(propertyMetadata) is IPropertyOrConstant maxValue)
            return new MaxValueMetadataDto() { Value = maxValue.Build() };
        return null;
    }

    private IInputMetadataDto? ProcessMinValue<T>(IMetadataBuilder<T> propertyMetadata)
    {
        Type? rangeInterface = propertyMetadata.GetType().GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValueRangable<,,>));

        PropertyInfo? maxValueProperty = rangeInterface?.GetProperty(nameof(IValueRangable<,>.MinValue));
        if (maxValueProperty?.GetValue(propertyMetadata) is IPropertyOrConstant minValue)
            return new MaxValueMetadataDto() { Value = minValue.Build() };
        return null;
    }
}
