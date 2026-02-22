using System.Text.Json;
using FormsApi.Common.Registry;
using FormsApi.Form.Primitives;

namespace Tests.Repository;

public class RepositoryTypeTests
{
    [Test]
    public void TestSerialization()
    {
        var registry = new RepositoryTypeRegistry();

        RepositoryType type = registry.Add<RepositoryTypeTests>();

        string serialized = JsonSerializer.Serialize(type);
        RepositoryType result = JsonSerializer.Deserialize<RepositoryType>(serialized);
        Assert.That(result, Is.EqualTo(type));
    }

    [Test]
    public void TestConversion()
    {
        var registry = new RepositoryTypeRegistry();

        RepositoryType type = registry.Add<RepositoryTypeTests>();
        Assert.That(typeof(RepositoryTypeTests), Is.EqualTo(registry.TryGetRuntimeType(type)));
    }
}
