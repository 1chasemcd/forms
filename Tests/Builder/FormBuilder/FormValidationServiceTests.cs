using FormsApi.Builder.Validation;
using FormsApi.Definition;
using FormsApi.Definition.Field;
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

    private readonly FormDefinition _form = new()
    {
        ModelType = new(typeof(FormValidationServiceTests)),
        View = new CombinedViewDefinition
        {
            Views = new BaseViewDefinition[]
            {
                new FieldViewDefinition
                {
                    Fields = new FieldDefinition[]
                    {
                        new() { Property = "Property1" },
                        new() { Property = "Property2" },
                        new() { Property = "Property3" }
                    }
                },
                new CombinedViewDefinition
                {
                    Views = new BaseViewDefinition[]
                    {
                        new FieldViewDefinition
                        {
                            Fields = new FieldDefinition[]
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
