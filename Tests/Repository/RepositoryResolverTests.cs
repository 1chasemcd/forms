using FormsApi.Repository;
using FormsApi.Repository.Query;
using FormsApi.Repository.Service;
using Moq;
using NUnit.Framework;

namespace Tests.Repository;

[TestFixture]
public class RepositoryResolverTests
{
    private Mock<IServiceProvider> _providerMock = null!;
    private IRepositoryResolver _resolver = null!;

    [SetUp]
    public void Setup()
    {
        _providerMock = new Mock<IServiceProvider>();
        _resolver = new RepositoryResolver(_providerMock.Object);
    }

    [Test]
    public void Resolve_ReturnsRepository_ForExactType()
    {
        object repo = new TestRepository<TestModelBase>();

        _providerMock
            .Setup(p => p.GetService(typeof(IRepository<TestModelBase>)))
            .Returns(repo);

        object result = _resolver.Resolve(typeof(TestModelBase));

        Assert.That(result, Is.SameAs(repo));
    }

    [Test]
    public void Resolve_ReturnsRepository_ForBaseType()
    {
        object repo = new TestRepository<TestModelBase>();

        _providerMock
            .Setup(p => p.GetService(typeof(IRepository<TestModelBase>)))
            .Returns(repo);

        object result = _resolver.Resolve(typeof(TestModelChild));

        Assert.That(result, Is.SameAs(repo));
    }

    [Test]
    public void Resolve_ReturnsRepository_ForInterface()
    {
        object repo = new TestRepository<ITestModel>();

        _providerMock
            .Setup(p => p.GetService(typeof(IRepository<ITestModel>)))
            .Returns(repo);

        object result = _resolver.Resolve(typeof(TestModelChild));

        Assert.That(result, Is.SameAs(repo));
    }

    [Test]
    public void Resolve_ReturnsDefault_ForUnregisteredType()
    {
        object result = _resolver.Resolve(typeof(TestModelBase));
        Assert.That(result, Is.InstanceOf<DefaultRepository<TestModelBase>>());
    }

    [Test]
    public void Resolve_ThrowsException_ForTypeWithoutDefaultConstructor()
    {
        Assert.Throws<InvalidOperationException>(() => _resolver.Resolve(typeof(TestModelNoDefaultConstructor)));
    }

    private class TestModelBase;
    private class TestModelChild : TestModelBase, ITestModel;
    private interface ITestModel;
    private class TestModelNoDefaultConstructor(int i) { private readonly int _i = i; }
    private class TestRepository<T> : IRepository<T>
    {
        public Task DeleteAsync(T toDelete) => throw new NotImplementedException();
        public Task<IEnumerable<T>> GetAsync(QueryCriteria criteria) => throw new NotImplementedException();
        public Task<T> GetNewAsync() => throw new NotImplementedException();
        public Task SaveAsync(T toSave) => throw new NotImplementedException();
    }
}
