using System.Text.Json;
using FormsApi.Form.Primitives;
using FormsApi.Repository;
using FormsApi.Repository.Handler;
using FormsApi.Repository.Query;
using FormsApi.Repository.Service;
using Moq;

namespace Tests.Repository;

public class RepositoryServiceFactoryTests
{
    private RepositoryServiceFactory _factory;
    private Mock<IRepositoryCreateHandler<TestModel>> _repoMock;

    [OneTimeSetUp]
    public void SetUp()
    {
        _repoMock = new Mock<IRepositoryCreateHandler<TestModel>>();
        _repoMock
            .Setup(r => r.CreateAsync())
            .ReturnsAsync(new TestModel());
        _repoMock.As<IRepositorySaveHandler<TestModel>>()
            .Setup(r => r.SaveAsync(It.IsAny<TestModel>()));

        _repoMock.As<IRepositoryQueryHandler<TestModel>>()
            .Setup(r => r.QueryAsync(It.IsAny<QueryCriteria>()))
            .ReturnsAsync(new List<TestModel>());
        _repoMock.As<IRepositoryDeleteHandler<TestModel>>()
            .Setup(r => r.DeleteAsync(It.IsAny<TestModel>()));

        var resolver = new Mock<IRepositoryResolver>();
        resolver
            .Setup(r => r.Resolve(It.IsAny<Type>(), typeof(TestModel)))
            .Returns(_repoMock.Object);

        _factory = new RepositoryServiceFactory(resolver.Object);
    }

    [Test]
    public void BuildCreateService_ReturnsCreateService()
    {
        var type = new SerializedType(typeof(TestModel));
        IRepositoryCallable service = _factory.BuildCreateService(type);
        Assert.That(service, Is.InstanceOf<RepositoryCreateService<TestModel>>());

        service.Invoke();
        _repoMock.Verify(r => r.CreateAsync(), Times.Once);
    }

    [Test]
    public void BuildQueryService_ReturnsQueryService()
    {
        var type = new SerializedType(typeof(TestModel));
        var criteria = new QueryCriteria();
        IRepositoryCallable service = _factory.BuildQueryService(type, criteria);
        Assert.That(service, Is.InstanceOf<RepositoryQueryService<TestModel>>());

        service.Invoke();
        _repoMock.As<IRepositoryQueryHandler<TestModel>>().Verify(r => r.QueryAsync(criteria), Times.Once);
    }

    [Test]
    public void BuildDeleteService_ReturnsDeleteService()
    {
        var type = new SerializedType(typeof(TestModel));
        TestModel model = new();
        IRepositoryCallable service = _factory.BuildDeleteService(type, model);
        Assert.That(service, Is.InstanceOf<RepositoryDeleteService<TestModel>>());

        service.Invoke();
        _repoMock.As<IRepositoryDeleteHandler<TestModel>>().Verify(r => r.DeleteAsync(model), Times.Once);
    }

    [Test]
    public void BuildSaveService_ReturnsSaveService()
    {
        var type = new SerializedType(typeof(TestModel));
        TestModel model = new();
        IRepositoryCallable service = _factory.BuildSaveService(type, model);
        Assert.That(service, Is.InstanceOf<RepositorySaveService<TestModel>>());

        service.Invoke();
        _repoMock.As<IRepositorySaveHandler<TestModel>>().Verify(r => r.SaveAsync(model), Times.Once);
    }
    public class TestModel;
}
