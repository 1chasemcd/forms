using System.Numerics;
using System.Reflection;
using FormsApi.Builder;
using FormsApi.Builder.Metadata;
using FormsApi.Definition.InputMetadata;
using FormsApi.Definition.Metadata;
using FormsApi.Definition.Primitives;
using FormsApi.Definition.Service;

public sealed class MetadataBuilderService(MetadataProcessors metadataProcessors)
{
    private Dictionary<Type, Type> _metadataDefinitions = [];
    public void BuildMetadataDictionary()
    {
        IEnumerable<Type> metadatas = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && ModelOfMetadata(x) is not null);

        _metadataDefinitions = metadatas.ToDictionary(x => ModelOfMetadata(x)!);
    }

    static Type? ModelOfMetadata(Type type)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Metadata<>))
                return type.GetGenericArguments()[0];

            type = type.BaseType!;
        }

        return null;
    }

    public List<ModelMetadataDto> BuildMetadata(Type baseType)
    {
        ArgumentNullException.ThrowIfNull(baseType);
        MethodInfo method = typeof(MetadataBuilderService)
            .GetMethod(nameof(BuildMetadataImpl), BindingFlags.Instance | BindingFlags.NonPublic)!
            .MakeGenericMethod(baseType)!;
        return (List<ModelMetadataDto>)method.Invoke(this, null)!;
    }

    private List<ModelMetadataDto> BuildMetadataImpl<T>()
    {
        List<ModelMetadataDto> result = [];

        var modelMetadata = new ModelMetadataDto()
        {
            Type = new TypeDto(typeof(T)),
            PropertyMetadatas = []
        };
        result.Add(modelMetadata);

        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            IPropertyMetadataDto propertyMetadata;
            if (IsPrimitiveType(prop.PropertyType))
            {
                propertyMetadata = BuildPrimitiveMetadata<T>(prop);
            }
            else if (GetElementType(prop.PropertyType) is { } elementType)
            {
                propertyMetadata = new EnumerableMetadataDto(elementType);
                result.AddRange(BuildMetadata(elementType));
            }
            else
            {
                propertyMetadata = new SubPropertyMetadataDto(prop.PropertyType);
                result.AddRange(BuildMetadata(prop.PropertyType));
            }

            modelMetadata.PropertyMetadatas.Add(prop.Name, propertyMetadata);
        }
        return result;
    }

    private PrimitiveMetadataDto BuildPrimitiveMetadata<T>(PropertyInfo prop)
    {
        Metadata<T>? metadataDefinition = null;
        if (_metadataDefinitions.TryGetValue(typeof(T), out Type? defType))
            metadataDefinition = Activator.CreateInstance(defType) as Metadata<T>;
        IMetadataBuilder<T>? propertyMetadata = null;
        metadataDefinition?.MetadataBuilders.TryGetValue(prop.Name, out propertyMetadata);
        if (propertyMetadata is null)
            return new PrimitiveMetadataDto()
            {
                Metadatas = new[]
                {
                    new InputTypeMetadataDto() { Value = GetDefaultInputType(prop.PropertyType) }
                }
            };

        List<IInputMetadataDto> metadatas = [];

        foreach (Func<IMetadataBuilder<T>, IInputMetadataDto?> processor in metadataProcessors.GetProcessors<T>())
        {
            IInputMetadataDto? processed = processor.Invoke(propertyMetadata);
            if (processed is not null) metadatas.Add(processed);
        }

        return new PrimitiveMetadataDto() { Metadatas = metadatas };
    }

    private static InputType GetDefaultInputType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (type == typeof(bool)) return InputType.CheckBox;
        if (type == typeof(DateOnly)) return InputType.Date;
        if (type == typeof(TimeOnly)) return InputType.Time;
        if (type == typeof(string)) return InputType.Text;
        if (type.IsAssignableFrom(typeof(INumber<>))) return InputType.Numeric;

        return InputType.Text;
    }

    private static Type? GetElementType(Type type)
    {
        if (type.IsArray)
        {
            return type.GetElementType();
        }

        var enumerableInterface = type
            .GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        if (enumerableInterface != null)
        {
            return enumerableInterface.GetGenericArguments()[0];
        }

        return null;
    }

    private static bool IsPrimitiveType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        return
            type.IsPrimitive ||
            type == typeof(string) ||
            type == typeof(decimal) ||
            type == typeof(DateOnly) ||
            type == typeof(TimeOnly) ||
            type == typeof(Guid);
    }
}
