using System.Text.Json;
using FormsApi.Definition.Primitives;

namespace Tests.Repository;

public class RepositoryTypeTests
{
    [Test]
    public void TestSerialization()
    {

        SerializedType type = new(typeof(RepositoryTypeTests));

        string serialized = JsonSerializer.Serialize(type);
        SerializedType? result = JsonSerializer.Deserialize<SerializedType>(serialized);
        Assert.That(result, Is.EqualTo(type));
    }

    [Test]
    public void TestConversion()
    {
        SerializedType type = new(typeof(RepositoryTypeTests));
        Assert.That(typeof(RepositoryTypeTests), Is.EqualTo(type.GetRuntimeType()));
    }
}
