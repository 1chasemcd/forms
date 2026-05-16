using System.Text.Json;
using FormsApi.Definition.Primitives;

namespace Tests.Repository;

public class RepositoryTypeTests
{
    [Test]
    public void TestSerialization()
    {

        TypeDto type = new(typeof(RepositoryTypeTests));

        string serialized = JsonSerializer.Serialize(type);
        TypeDto? result = JsonSerializer.Deserialize<TypeDto>(serialized);
        Assert.That(result, Is.EqualTo(type));
    }

    [Test]
    public void TestConversion()
    {
        TypeDto type = new(typeof(RepositoryTypeTests));
        Assert.That(typeof(RepositoryTypeTests), Is.EqualTo(type.GetRuntimeType()));
    }
}
