using FormsApi.Definition;
using FormsApi.Definition.Input;
using FormsApi.Definition.View;

namespace Tests.Builder.FormBuilder;

public class FormValidationServiceTests
{
    readonly FormValidationService _service = new();
    [Test]
    public void Test1()
    {
        FormValidationService.InvalidFormException? exception =
            Assert.Throws<FormValidationService.InvalidFormException>(() => _service.Validate(_form));

        Assert.That(exception.Message, Does.Contain("Property1"));
        Assert.That(exception.Message, Does.Contain("Property3"));
        Assert.That(exception.Message, Does.Not.Contain("Property2"));
    }

    private readonly FormDto _form = new()
    {
        ModelType = new(typeof(FormValidationServiceTests)),
        View = new CombinedViewDto
        {
            Views = new BaseViewDto[]
            {
                new FieldViewDto
                {
                    Fields = new FieldDto[]
                    {
                        new() { Property = "Property1" },
                        new() { Property = "Property2" },
                        new() { Property = "Property3" }
                    }
                },
                new CombinedViewDto
                {
                    Views = new BaseViewDto[]
                    {
                        new FieldViewDto
                        {
                            Fields = new FieldDto[]
                            {
                                new() { Property = "Property1" },
                                new() { Property = "Property3" }
                            }
                        }
                    }
                }
            }
        }
    };
}
