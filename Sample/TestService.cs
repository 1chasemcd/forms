using System;
using FormsApi.Recalculate;

namespace Sample;

public class TestService
{
    public RecalculateEventResult SetNumericValue(TestModel model)
    {
        model.NumericField = 12345;
        return new RecalculateEventResult()
        {
            Model = model
        };
    }

    public RecalculateEventResult ResetForm()
    {
        return new RecalculateEventResult()
        {
            Model = new TestModel()
        };
    }
}
