using System;
using FormsApi.Common.Registry;
using FormsApi.Form.Primitives;
using FormsApi.Repository;
using FormsApi.Repository.Query;
using FormsApi.Repository.Service;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests.Repository;

public class RepositoryServiceBuilderTests
{
    private RepositoryRegistry _registry;
    private RepositoryServiceBuilder _builder;

    [SetUp]
    public void SetUp()
    {
        _registry = new RepositoryRegistry(NullLogger<RepositoryRegistry>.Instance);
        _builder = new RepositoryServiceBuilder(_registry);
    }

    [Test]
    public void BuildWithType_TypeWithNoDefaultConstructor_Throws()
    {
        var type = new RepositoryType(typeof(TestModelNoDefaultConstructor));
        Assert.Throws<Exception>(() => _builder.BuildWithType(type));
    }

    [Test]
    public void BuildWithType_TypeNotRegistered_ReturnsDefaultRepository()
    {
        var type = new RepositoryType(typeof(TestModel));
        IReadableRepositoryService service = _builder.BuildWithType(type);
        Assert.That(service, Is.InstanceOf<ReadableRepositoryService<TestModel>>()
            .With.Property(nameof(ReadableRepositoryService<>.Repository))
            .With.InstanceOf<DefaultRepository<TestModel>>());
    }

    [Test]
    public void BuildWithType_TypeRegistered_ReturnsRegisteredRepository()
    {
        _registry.Add(typeof(TestModel), new TestRepository());
        var type = new RepositoryType(typeof(TestModelChild));
        IReadableRepositoryService service = _builder.BuildWithType(type);
        Assert.That(service, Is.InstanceOf<ReadableRepositoryService<TestModel>>()
            .With.Property(nameof(ReadableRepositoryService<>.Repository))
            .With.InstanceOf<TestRepository>());
    }

    [Test]
    public void BuildWithTypeAndObject_TypeWithNoDefaultConstructor_Throws()
    {
        var type = new RepositoryType(typeof(TestModelNoDefaultConstructor));
        Assert.Throws<Exception>(() => _builder.BuildWithTypeAndObject(type, new TestModelNoDefaultConstructor(0)));
    }

    [Test]
    public void BuildWithTypeAndObject_TypeNotRegistered_ReturnsDefaultRepository()
    {
        var type = new RepositoryType(typeof(TestModel));
        IWriteableRepositoryService service = _builder.BuildWithTypeAndObject(type, new TestModel());
        var typedService = service as WriteableRepositoryService<TestModel, TestModel>;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(typedService, Is.Not.Null);
            Assert.That(typedService, Has.Property(nameof(WriteableRepositoryService<,>.Repository))
                .With.InstanceOf<DefaultRepository<TestModel>>());
            Assert.That(typedService, Has.Property(nameof(WriteableRepositoryService<,>.Object))
                .With.InstanceOf<TestModel>());
        }
    }

    [Test]
    public void BuildWithTypeAndObject_TypeRegistered_ReturnsRegisteredRepository()
    {
        _registry.Add(typeof(TestModel), new TestRepository());
        var type = new RepositoryType(typeof(TestModelChild));
        IWriteableRepositoryService service = _builder.BuildWithTypeAndObject(type, new TestModelChild());
        var typedService = service as WriteableRepositoryService<TestModel, TestModelChild>;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(typedService, Is.Not.Null);
            Assert.That(typedService, Has.Property(nameof(WriteableRepositoryService<,>.Repository))
                .With.InstanceOf<TestRepository>());
            Assert.That(typedService, Has.Property(nameof(WriteableRepositoryService<,>.Object))
                .With.InstanceOf<TestModelChild>());
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
    private class TestModelChild : TestModel;
    private class TestModelNoDefaultConstructor(int value)
    {
        private readonly int _value = value;
    }
}
