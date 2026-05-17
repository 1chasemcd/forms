using FormsApi.Common;
using FormsApi.Contract;

namespace Tests.Common
{
    [TestFixture]
    public class PropertyOrConstantTests
    {
        private sealed class TestModel
        {
            public string? Name { get; set; }
            public int Age { get; set; }
        }

        [Test]
        public void Build_WhenConstructedWithPropertySelector_ReturnsPropertyDto()
        {
            var sut = new PropertyOrConstant<TestModel, string>(x => x.Name);
            PropertyOrConstantDto result = sut.Build();
            Assert.That(result, Is.TypeOf<PropertyDto>());

            var property = (PropertyDto)result;
            Assert.That(property.Value, Is.EqualTo(nameof(TestModel.Name)));
        }

        [Test]
        public void Build_WhenConstructedWithConstantValue_ReturnsConstantDto()
        {
            var sut = new PropertyOrConstant<TestModel, string>("test");
            PropertyOrConstantDto result = sut.Build();
            Assert.That(result, Is.TypeOf<ConstantDto>());

            var constant = (ConstantDto)result;
            Assert.That(constant.Value, Is.EqualTo("test"));
        }

        [Test]
        public void Build_WhenConstructedUsingImplicitConstantConversion_ReturnsConstantDto()
        {
            PropertyOrConstant<TestModel, int> sut = 42;
            PropertyOrConstantDto result = sut.Build();
            Assert.That(result, Is.TypeOf<ConstantDto>());

            var constant = (ConstantDto)result;
            Assert.That(constant.Value, Is.EqualTo(42));
        }
    }
}
