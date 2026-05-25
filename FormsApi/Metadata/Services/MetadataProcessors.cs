using System.Reflection;
using FormsApi.Common;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Builders;
using FormsApi.Metadata.Interfaces;

namespace FormsApi.Metadata.Services;

internal sealed class MetadataProcessors
{
    public IEnumerable<Func<IMetadataBuilder<T>, PropertyMetadata?>> GetProcessors<T>()
    {
        yield return propertyMetadata =>
            new ControlTypeMetadata() { Value = propertyMetadata.GetControlType() };

        yield return propertyMetadata =>
            propertyMetadata is IEnablable<T> x && x.Enabled is not null
            ? new EnabledMetadata() { Value = x.Enabled.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is ILabelable<T> x && x.Label is not null
            ? new LabelMetadata() { Value = x.Label.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is IMaxLengthable<T> x && x.MaxLength is not null
            ? new MaxLengthMetadata() { Value = x.MaxLength.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is IPrecisionAndScalable<T> x && x.Precision is not null
            ? new PrecisionMetadata() { Value = x.Precision.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is IPrecisionAndScalable<T> x && x.Scale is not null
            ? new ScaleMetadata() { Value = x.Scale.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is IServiceMethodCaller<T> x && x.ServiceMethod is not null
            ? new ServiceMethodMetadata() { Value = x.ServiceMethod.Build() }
            : null;

        yield return propertyMetadata =>
            propertyMetadata is IRequirable<T> x && x.Required is not null
            ? new RequiredMetadata() { Value = x.Required.Build() }
            : null;

        yield return ProcessMaxValue;
        yield return ProcessMinValue;

        yield return propertyMetadata =>
            propertyMetadata is IVisible<T> x && x.Visible is not null
            ? new VisibleMetadata() { Value = x.Visible.Build() }
            : null;
    }

    private static MaxValueMetadata? ProcessMaxValue<T>(IMetadataBuilder<T> propertyMetadata)
    {
        Type? rangeInterface = propertyMetadata.GetType().GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValueRangable<,>));

        PropertyInfo? maxValueProperty = rangeInterface?.GetProperty(nameof(IValueRangable<,>.MaxValue));
        if (maxValueProperty?.GetValue(propertyMetadata) is IPropertyOrConstantBuilder maxValue)
            return new MaxValueMetadata() { Value = maxValue.Build() };
        return null;
    }

    private static MinValueMetadata? ProcessMinValue<T>(IMetadataBuilder<T> propertyMetadata)
    {
        Type? rangeInterface = propertyMetadata.GetType().GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValueRangable<,>));

        PropertyInfo? minValueProperty = rangeInterface?.GetProperty(nameof(IValueRangable<,>.MinValue));
        if (minValueProperty?.GetValue(propertyMetadata) is IPropertyOrConstantBuilder minValue)
            return new MinValueMetadata() { Value = minValue.Build() };
        return null;
    }
}
