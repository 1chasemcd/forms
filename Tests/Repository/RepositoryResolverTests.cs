using FormsApi.Repository.Handler;
using FormsApi.Repository.Query;
using FormsApi.Repository.Service;
using Moq;

namespace Tests.Repository;

[TestFixture]
public class RepositoryResolverTests
{
    private Mock<IServiceProvider> _providerMock = null!;
    private RepositoryResolver _resolver = null!;

    [SetUp]
    public void Setup()
    {
        _providerMock = new Mock<IServiceProvider>();
        _resolver = new RepositoryResolver(_providerMock.Object);
    }

    [TestCase(typeof(IRepositoryCreateHandler<TestModelBase>))]
    [TestCase(typeof(IRepositoryDeleteHandler<TestModelBase>))]
    [TestCase(typeof(IRepositoryQueryHandler<TestModelBase>))]
    [TestCase(typeof(IRepositorySaveHandler<TestModelBase>))]
    public void Resolve_ReturnsRepository_ForExactType(Type interfaceToResolve)
    {
        object repo = new TestRepository<TestModelBase>();

        _providerMock
            .Setup(p => p.GetService(interfaceToResolve))
            .Returns(repo);

        object result = _resolver.Resolve(interfaceToResolve.GetGenericTypeDefinition(), typeof(TestModelBase));

        Assert.That(result, Is.SameAs(repo));
    }

    [TestCase(typeof(IRepositoryCreateHandler<TestModelBase>))]
    [TestCase(typeof(IRepositoryDeleteHandler<TestModelBase>))]
    [TestCase(typeof(IRepositoryQueryHandler<TestModelBase>))]
    [TestCase(typeof(IRepositorySaveHandler<TestModelBase>))]
    public void Resolve_ReturnsRepository_ForBaseType(Type interfaceToResolve)
    {
        object repo = new TestRepository<TestModelBase>();

        _providerMock
            .Setup(p => p.GetService(interfaceToResolve))
            .Returns(repo);

        object result = _resolver.Resolve(interfaceToResolve.GetGenericTypeDefinition(), typeof(TestModelChild));

        Assert.That(result, Is.SameAs(repo));
    }

    [TestCase(typeof(IRepositoryCreateHandler<ITestModel>))]
    [TestCase(typeof(IRepositoryDeleteHandler<ITestModel>))]
    [TestCase(typeof(IRepositoryQueryHandler<ITestModel>))]
    [TestCase(typeof(IRepositorySaveHandler<ITestModel>))]
    public void Resolve_ReturnsRepository_ForInterface(Type interfaceToResolve)
    {
        object repo = new TestRepository<ITestModel>();

        _providerMock
            .Setup(p => p.GetService(interfaceToResolve))
            .Returns(repo);

        object result = _resolver.Resolve(interfaceToResolve.GetGenericTypeDefinition(), typeof(TestModelChild));

        Assert.That(result, Is.SameAs(repo));
    }

    [Test]
    public void Resolve_ReturnsDefault_ForUnregisteredType()
    {
        object result = _resolver.Resolve(typeof(IRepositoryCreateHandler<>), typeof(TestModelBase));
        Assert.That(result, Is.InstanceOf<DefaultRepositoryCreateHandler<TestModelBase>>());
    }

    [Test]
    public void Resolve_ThrowsException_ForTypeWithoutDefaultConstructor()
    {
        Assert.Throws<InvalidOperationException>(() => _resolver.Resolve(typeof(IRepositoryCreateHandler<>), typeof(TestModelNoDefaultConstructor)));
    }

    private class TestModelBase;
    private class TestModelChild : TestModelBase, ITestModel;
    private interface ITestModel;
    private class TestModelNoDefaultConstructor(int i) { private readonly int _i = i; }
    private class TestRepository<T> : IRepositoryCreateHandler<T>, IRepositorySaveHandler<T>, IRepositoryQueryHandler<T>, IRepositoryDeleteHandler<T>
    {
        public Task DeleteAsync(T toDelete) => throw new NotImplementedException();
        public Task<IEnumerable<T>> QueryAsync(QueryCriteria criteria) => throw new NotImplementedException();
        public Task<T> CreateAsync() => throw new NotImplementedException();
        public Task SaveAsync(T toSave) => throw new NotImplementedException();
    }
}
