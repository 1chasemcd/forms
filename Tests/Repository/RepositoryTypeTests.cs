using System.Text.Json;
using FormsApi.Repository;

namespace Tests.Repository;

public class RepositoryTypeTests
{
    [Test]
    public void TestSerialization()
    {
        var testType = typeof(RepositoryTypeTests);

        var type = new RepositoryType(testType);
        var serialized = JsonSerializer.Serialize(type);
        var result = JsonSerializer.Deserialize<RepositoryType>(serialized);
        Assert.That(result.GetRepositoryType(), Is.EqualTo(testType));
    }

    [Test]
    public void TestConversion()
    {
        var testType = typeof(RepositoryTypeTests);

        var type = new RepositoryType(testType);
        Assert.That(type.GetRepositoryType(), Is.EqualTo(testType));
    }
}
