using System.Text.Json;
using FormsApi.Common.Registry;
using FormsApi.Form.Primitives;

namespace Tests.Repository;

public class RepositoryTypeTests
{
    [Test]
    public void TestSerialization()
    {

        RepositoryType type = new(typeof(RepositoryTypeTests));

        string serialized = JsonSerializer.Serialize(type);
        RepositoryType? result = JsonSerializer.Deserialize<RepositoryType>(serialized);
        Assert.That(result, Is.EqualTo(type));
    }

    [Test]
    public void TestConversion()
    {
        RepositoryType type = new(typeof(RepositoryTypeTests));
        Assert.That(typeof(RepositoryTypeTests), Is.EqualTo(type.GetRuntimeType()));
    }
}
