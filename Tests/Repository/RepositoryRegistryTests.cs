using FormsApi.Common.Registry;
using FormsApi.Repository;
using FormsApi.Repository.Query;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests.Repository;

public class RepositoryRegistryTests
{
    private RepositoryRegistry _registry;

    [SetUp]
    public void SetUp()
    {
        _registry = new(NullLogger<RepositoryRegistry>.Instance);
    }

    [Test]
    public void TryGet_GetsRepositoryForType()
    {
        _registry.Add(typeof(TestModelBase), new TestRepositoryBase());
        object? result = _registry.TryGet(typeof(TestModelBase));
        Assert.That(result, Is.InstanceOf<TestRepositoryBase>());
    }

    [Test]
    public void TryGet_SingleAncestor_GetsRepositoryForAncestorType()
    {
        _registry.Add(typeof(TestModelBase), new TestRepositoryBase());
        object? result = _registry.TryGet(typeof(TestModelChild));
        Assert.That(result, Is.InstanceOf<TestRepositoryBase>());
    }

    [Test]
    public void TryGet_MultipleAncestors_GetsRepositoryForClosestType()
    {
        _registry.Add(typeof(TestModelBase), new TestRepositoryBase());
        _registry.Add(typeof(TestModelChild), new TestRepositoryChild());

        object? result = _registry.TryGet(typeof(TestModelChildChild));
        Assert.That(result, Is.InstanceOf<TestRepositoryChild>());
    }


    [Test]
    public void TryGet_NoAncestors_GetsNull()
    {
        object? result = _registry.TryGet(typeof(TestModelBase));
        Assert.That(result, Is.Null);
    }

    private class TestModelBase { }
    private class TestModelChild : TestModelBase { }
    private class TestModelChildChild : TestModelChild { }
    private class TestRepositoryBase : IRepository<TestModelBase>
    {
        public Task DeleteAsync(TestModelBase toDelete) => throw new NotImplementedException();
        public Task<IEnumerable<TestModelBase>> GetAsync(QueryCriteria criteria) => throw new NotImplementedException();
        public Task<TestModelBase> GetNewAsync() => throw new NotImplementedException();
        public Task SaveAsync(TestModelBase toSave) => throw new NotImplementedException();
    }

    private class TestRepositoryChild : IRepository<TestModelChild>
    {
        public Task DeleteAsync(TestModelChild toDelete) => throw new NotImplementedException();
        public Task<IEnumerable<TestModelChild>> GetAsync(QueryCriteria criteria) => throw new NotImplementedException();
        public Task<TestModelChild> GetNewAsync() => throw new NotImplementedException();
        public Task SaveAsync(TestModelChild toSave) => throw new NotImplementedException();
    }
}
