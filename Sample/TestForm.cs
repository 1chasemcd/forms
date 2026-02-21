using System;
using FormsApi.Builder;
using FormsApi.Builder.View;

namespace Sample;

public class TestForm : FormBuilder<TestModel>
{
    protected override ViewBuilder<TestModel> View => new CombinedViewBuilder<TestModel>("Test Form")
    {
        CreateDataView()
    };

    public DataViewBuilder<TestModel> CreateDataView()
    {
        return new DataViewBuilder<TestModel>()
        {
            { m => m.TextField, p => p.WithWidth(50) },
            { m => m.DateField, p => p.WithWidth(50) },
            { m => m.NumericField, p => p.WithWidth(50) },
            { m => m.CurrencyField, p => p.WithWidth(50) },
        };
    }
}
