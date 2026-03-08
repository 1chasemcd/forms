using System;
using FormsApi.FormAction;

namespace Sample;

public class TestService
{
    public FormActionResult SetNumericValue(TestModel model)
    {
        model.NumericField = 12345;
        return new FormActionResult()
        {
            Model = model
        };
    }

    public FormActionResult ResetForm()
    {
        return new FormActionResult()
        {
            Model = new TestModel()
        };
    }
}
