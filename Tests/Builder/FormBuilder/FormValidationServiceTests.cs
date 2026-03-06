using FormsApi.Builder.Validation;
using FormsApi.Form;
using FormsApi.Form.Field;
using FormsApi.Form.View;
using NUnit.Framework;

namespace Tests.Builder.FormBuilder;

public class FormValidationServiceTests
{
    FormValidationService _service = new();
    [Test]
    public void Test1()
    {
        FormValidationService.InvalidFormException? exception =
            Assert.Throws<FormValidationService.InvalidFormException>(() => _service.Validate(_form));

        Assert.That(exception.Message, Does.Contain("Property1"));
        Assert.That(exception.Message, Does.Contain("Property3"));
        Assert.That(exception.Message, Does.Not.Contain("Property2"));
    }

    private readonly FormModel _form = new()
    {
        Type = new(typeof(FormValidationServiceTests)),
        View = new CombinedView
        {
            Views = new BaseView[]
            {
                new DataView
                {
                    Fields = new BaseField[]
                    {
                        new TextInput() { Property = "Property1" },
                        new TextInput() { Property = "Property2" },
                        new TextInput() { Property = "Property3" }
                    }
                },
                new CombinedView
                {
                    Views = new BaseView[]
                    {
                        new DataView
                        {
                            Fields = new BaseField[]
                            {
                                new TextInput() { Property = "Property1" },
                                new TextInput() { Property = "Property3" }
                            }
                        }
                    }
                }
            }
        }
    };
}
