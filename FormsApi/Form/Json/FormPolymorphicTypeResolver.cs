using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace FormsApi.Form.Json;

internal class FormPolymorphicTypeResolver : DefaultJsonTypeInfoResolver
{
    private readonly Dictionary<Type, Type[]> _derivedTypeMap = [];
    public FormPolymorphicTypeResolver()
    {
        Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type type in GetTypesToResolve(allTypes))
            _derivedTypeMap[type] = [.. GetDerivedTypes(allTypes, type)];
    }

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (_derivedTypeMap.TryGetValue(type, out Type[]? derivedTypes)
            && derivedTypes.Length > 0)
            jsonTypeInfo.PolymorphismOptions = OptionsForType(type);

        return jsonTypeInfo;
    }

    private static IEnumerable<Type> GetTypesToResolve(Type[] allTypes)
    {
        string formNamespace = typeof(FormModel).Namespace ?? string.Empty;
        return allTypes.Where(
            t => t.IsAbstract &&
            t.Namespace is string ns &&
            ns.StartsWith(formNamespace) &&
            !t.IsDefined(typeof(FormPolymorphicResolverIgnoreAttribute), inherit: false));
    }

    private static IEnumerable<Type> GetDerivedTypes(Type[] allTypes, Type baseType)
    {
        return allTypes.Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t));
    }

    private JsonPolymorphismOptions OptionsForType(Type baseType)
    {
        var polymorphism = new JsonPolymorphismOptions()
        {
            IgnoreUnrecognizedTypeDiscriminators = true,
            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
        };

        foreach (Type type in _derivedTypeMap[baseType])
        {
            polymorphism.DerivedTypes.Add(
                new JsonDerivedType(type, type.Name.ToLowerInvariant())
            );
        }

        return polymorphism;
    }
}
