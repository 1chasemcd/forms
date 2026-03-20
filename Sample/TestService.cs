using System;
using FormsApi.Recalculate;

namespace Sample;

public class TestService
{
    public RecalculateEventResult<TestModel> SetNumericValue(TestModel model)
    {
        model.NumericField = 12345;
        return new RecalculateEventResult<TestModel>()
        {
            Model = model
        };
    }

    public RecalculateEventResult<TestModel> ResetForm()
    {
        return new RecalculateEventResult<TestModel>()
        {
            Model = new TestModel()
        };
    }
}
