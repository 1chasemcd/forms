using System.Numerics;
using System.Reflection;
using FormsApi.Contract;
using FormsApi.Contract.ControlMetadata;
using FormsApi.Contract.MetadataCollection;
using FormsApi.Metadata.Builders;

namespace FormsApi.Metadata.Services;

public interface IMetadataBuilderService
{
    void CollectMetadataDictionary();
    List<ModelMetadataCollectionDto> BuildMetadata(Type baseType);
}

internal sealed class MetadataBuilderService(MetadataProcessors metadataProcessors) : IMetadataBuilderService
{
    private Dictionary<Type, Type> _metadataDefinitions = [];
    public void CollectMetadataDictionary()
    {
        IEnumerable<Type> metadatas = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && ModelOfMetadata(x) is not null);

        _metadataDefinitions = metadatas.ToDictionary(x => ModelOfMetadata(x)!);
    }

    private static Type? ModelOfMetadata(Type type)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Metadata<>))
                return type.GetGenericArguments()[0];

            type = type.BaseType!;
        }

        return null;
    }

    public List<ModelMetadataCollectionDto> BuildMetadata(Type baseType)
    {
        ArgumentNullException.ThrowIfNull(baseType);
        ICollection<Type> alreadyBuilt = [];
        return CallBuildMetadata(baseType, alreadyBuilt);
    }

    private List<ModelMetadataCollectionDto> CallBuildMetadata(Type baseType, ICollection<Type> alreadyBuilt)
    {
        if (alreadyBuilt.Contains(baseType)) return [];
        alreadyBuilt.Add(baseType);
        MethodInfo method = typeof(MetadataBuilderService)
            .GetMethod(nameof(BuildMetadataImpl), BindingFlags.Instance | BindingFlags.NonPublic)!
            .MakeGenericMethod(baseType)!;
        return (List<ModelMetadataCollectionDto>)method.Invoke(this, new[] { alreadyBuilt })!;
    }

    private List<ModelMetadataCollectionDto> BuildMetadataImpl<T>(ICollection<Type> alreadyBuilt)
    {
        List<ModelMetadataCollectionDto> result = [];

        var modelMetadata = new ModelMetadataCollectionDto()
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
                propertyMetadata = new EnumerablePropertyMetadataDto(elementType);
                result.AddRange(CallBuildMetadata(elementType, alreadyBuilt));
            }
            else
            {
                propertyMetadata = new SubPropertyMetadataDto(prop.PropertyType);
                result.AddRange(CallBuildMetadata(prop.PropertyType, alreadyBuilt));
            }

            modelMetadata.PropertyMetadatas.Add(prop.Name, propertyMetadata);
        }
        return result;
    }

    private PrimitivePropertyMetadataDto BuildPrimitiveMetadata<T>(PropertyInfo prop)
    {
        Metadata<T>? metadataDefinition = null;
        if (_metadataDefinitions.TryGetValue(typeof(T), out Type? defType))
            metadataDefinition = Activator.CreateInstance(defType) as Metadata<T>;
        IMetadataBuilder<T>? propertyMetadata = null;
        metadataDefinition?.MetadataBuilders.TryGetValue(prop.Name, out propertyMetadata);
        if (propertyMetadata is null)
            return new PrimitivePropertyMetadataDto()
            {
                Metadatas = new[]
                {
                    new ControlTypeMetadataDto() { Value = GetDefaultInputType(prop.PropertyType) }
                }
            };

        List<BaseControlMetadataDto> metadatas = [];

        foreach (Func<IMetadataBuilder<T>, BaseControlMetadataDto?> processor in metadataProcessors.GetProcessors<T>())
        {
            BaseControlMetadataDto? processed = processor.Invoke(propertyMetadata);
            if (processed is not null) metadatas.Add(processed);
        }

        return new PrimitivePropertyMetadataDto() { Metadatas = metadatas };
    }

    private static ControlType GetDefaultInputType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (type == typeof(bool)) return ControlType.CheckBox;
        if (type == typeof(DateOnly)) return ControlType.Date;
        if (type == typeof(TimeOnly)) return ControlType.Time;
        if (type == typeof(string)) return ControlType.Text;
        if (type.IsAssignableFrom(typeof(INumber<>))) return ControlType.Numeric;

        return ControlType.Text;
    }

    private static Type? GetElementType(Type type)
    {
        if (type.IsArray)
        {
            return type.GetElementType();
        }

        Type? enumerableInterface = type
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
