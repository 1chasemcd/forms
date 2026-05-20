using System.Reflection;
using FormsApi.Common;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Metadata.Builders;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Services;

internal sealed class MetadataProcessors
{
    public IEnumerable<Func<IMetadataBuilder<T>, BaseControlMetadataDto?>> GetProcessors<T>()
    {
        yield return propertyMetadata =>
            new ControlTypeMetadataDto() { Value = propertyMetadata.GetControlType() };

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
            propertyMetadata is IFormServiceCaller<T> x && x.FormServiceMethod is not null
            ? new FormServiceMethodMetadataDto() { Value = x.FormServiceMethod.Build() }
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

    private static MaxValueMetadataDto? ProcessMaxValue<T>(IMetadataBuilder<T> propertyMetadata)
    {
        Type? rangeInterface = propertyMetadata.GetType().GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValueRangable<,>));

        PropertyInfo? maxValueProperty = rangeInterface?.GetProperty(nameof(IValueRangable<,>.MaxValue));
        if (maxValueProperty?.GetValue(propertyMetadata) is IPropertyOrConstant maxValue)
            return new MaxValueMetadataDto() { Value = maxValue.Build() };
        return null;
    }

    private static MinValueMetadataDto? ProcessMinValue<T>(IMetadataBuilder<T> propertyMetadata)
    {
        Type? rangeInterface = propertyMetadata.GetType().GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValueRangable<,>));

        PropertyInfo? minValueProperty = rangeInterface?.GetProperty(nameof(IValueRangable<,>.MinValue));
        if (minValueProperty?.GetValue(propertyMetadata) is IPropertyOrConstant minValue)
            return new MinValueMetadataDto() { Value = minValue.Build() };
        return null;
    }
}
