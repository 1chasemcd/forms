using System.Text.Json;
using FormsApi.Form.Primitives;
using FormsApi.Repository;
using FormsApi.Repository.Query;
using FormsApi.Repository.Service;
using Moq;

namespace Tests.Repository;

public class RepositoryServiceFactoryTests
{
    private RepositoryServiceFactory _factory;

    [OneTimeSetUp]
    public void SetUp()
    {
        var resolver = new Mock<IRepositoryResolver>();
        resolver
            .Setup(r => r.Resolve(typeof(TestModel)))
            .Returns(new TestRepository());
        _factory = new RepositoryServiceFactory(resolver.Object);
    }

    [Test]
    public void BuildWithType_TypeRegistered_ReturnsRegisteredRepository()
    {
        var type = new RepositoryType(typeof(TestModel));
        IReadableRepositoryService service = _factory.BuildWithType(type);
        Assert.That(service, Is.InstanceOf<ReadableRepositoryService<TestModel>>()
            .With.Property(nameof(ReadableRepositoryService<>.Repository))
            .With.InstanceOf<TestRepository>());
    }

    [Test]
    public void BuildWithTypeAndObject_TypeRegistered_ReturnsRegisteredRepository()
    {
        var type = new RepositoryType(typeof(TestModel));
        JsonElement obj = JsonSerializer.SerializeToElement(new TestModel());

        IWriteableRepositoryService service = _factory.BuildWithTypeAndObject(type, obj);
        var typedService = service as WriteableRepositoryService<TestModel, TestModel>;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(typedService, Is.Not.Null);
            Assert.That(typedService, Has.Property(nameof(WriteableRepositoryService<,>.Repository))
                .With.InstanceOf<TestRepository>());
            Assert.That(typedService, Has.Property(nameof(WriteableRepositoryService<,>.Object))
                .With.InstanceOf<TestModel>());
        }
    }

    private class TestRepository : IRepository<TestModel>
    {
        public Task DeleteAsync(TestModel toDelete) => throw new NotImplementedException();
        public Task<IEnumerable<TestModel>> GetAsync(QueryCriteria criteria) => throw new NotImplementedException();
        public Task<TestModel> GetNewAsync() => throw new NotImplementedException();
        public Task SaveAsync(TestModel toSave) => throw new NotImplementedException();
    }

    private class TestModel;
}
