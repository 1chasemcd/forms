using System.Numerics;
using System.Reflection;
using FormsApi.Contract;
using FormsApi.Contract.MetadataContainer;
using FormsApi.Contract.PropertyMetadata;
using FormsApi.Metadata.Builders;

namespace FormsApi.Metadata.Services;

public interface IMetadataBuilderService
{
    void CollectMetadataDictionary();
    List<ModelMetadataContainer> BuildMetadata(Type baseType);
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

    public List<ModelMetadataContainer> BuildMetadata(Type baseType)
    {
        ArgumentNullException.ThrowIfNull(baseType);
        ICollection<Type> alreadyBuilt = [];
        return CallBuildMetadata(baseType, alreadyBuilt);
    }

    private List<ModelMetadataContainer> CallBuildMetadata(Type baseType, ICollection<Type> alreadyBuilt)
    {
        if (alreadyBuilt.Contains(baseType)) return [];
        alreadyBuilt.Add(baseType);
        MethodInfo method = typeof(MetadataBuilderService)
            .GetMethod(nameof(BuildMetadataImpl), BindingFlags.Instance | BindingFlags.NonPublic)!
            .MakeGenericMethod(baseType)!;
        return (List<ModelMetadataContainer>)method.Invoke(this, new[] { alreadyBuilt })!;
    }

    private List<ModelMetadataContainer> BuildMetadataImpl<T>(ICollection<Type> alreadyBuilt)
    {
        List<ModelMetadataContainer> result = [];

        var modelMetadata = new ModelMetadataContainer()
        {
            Type = new TypeDto(typeof(T)),
            PropertyMetadatas = []
        };
        result.Add(modelMetadata);

        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            PropertyMetadataContainer propertyMetadata;
            if (IsPrimitiveType(prop.PropertyType))
            {
                propertyMetadata = BuildPrimitiveMetadata<T>(prop);
            }
            else if (GetElementType(prop.PropertyType) is { } elementType)
            {
                propertyMetadata = new ArrayMetadataContainer(elementType);
                result.AddRange(CallBuildMetadata(elementType, alreadyBuilt));
            }
            else
            {
                propertyMetadata = new SubPropertyMetadataContainer(prop.PropertyType);
                result.AddRange(CallBuildMetadata(prop.PropertyType, alreadyBuilt));
            }

            modelMetadata.PropertyMetadatas.Add(prop.Name, propertyMetadata);
        }
        return result;
    }

    private PrimitivePropertyMetadataContainer BuildPrimitiveMetadata<T>(PropertyInfo prop)
    {
        Metadata<T>? metadataDefinition = null;
        if (_metadataDefinitions.TryGetValue(typeof(T), out Type? defType))
            metadataDefinition = Activator.CreateInstance(defType) as Metadata<T>;
        IMetadataBuilder<T>? propertyMetadata = null;
        metadataDefinition?.MetadataBuilders.TryGetValue(prop.Name, out propertyMetadata);
        if (propertyMetadata is null)
            return new PrimitivePropertyMetadataContainer()
            {
                Metadatas = new[]
                {
                    new FieldTypeMetadata() { Value = GetDefaultInputType(prop.PropertyType) }
                }
            };

        List<PropertyMetadata> metadatas = [];

        foreach (Func<IMetadataBuilder<T>, PropertyMetadata?> processor in metadataProcessors.GetProcessors<T>())
        {
            PropertyMetadata? processed = processor.Invoke(propertyMetadata);
            if (processed is not null) metadatas.Add(processed);
        }

        return new PrimitivePropertyMetadataContainer() { Metadatas = metadatas };
    }

    private static FieldType GetDefaultInputType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (type == typeof(bool)) return FieldType.CheckBox;
        if (type == typeof(DateOnly)) return FieldType.Date;
        if (type == typeof(TimeOnly)) return FieldType.Time;
        if (type == typeof(string)) return FieldType.Text;
        if (type.IsAssignableFrom(typeof(INumber<>))) return FieldType.Numeric;

        return FieldType.Text;
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
