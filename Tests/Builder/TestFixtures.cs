using FormsApi.Builder;
using FormsApi.Builder.View;
using FormsApi.Common.Types;

namespace Tests.Builder;

public class TestFormBuilder : FormBuilder<TestModel>
{
    protected override ViewBuilder<TestModel> View => new CombinedViewBuilder<TestModel>("Title", 100)
    {
        DataView(),
        DataViewWithAugments(),
        SubPropertyGridView()
    };

    private static DataViewBuilder<TestModel> DataView()
    {
        return new DataViewBuilder<TestModel>() {
            { m => m.BoolProperty },
            { m => m.CurrencyProperty },
            { m => m.DateProperty },
            { m => m.DecimalProperty },
            { m => m.IntProperty },
            { m => m.StringListProperty },
            { m => m.StringProperty },
            { m => m.TimeProperty },

            { m => m.ButtonAction },
            { "Static Text" },
        };
    }

    private static DataViewBuilder<TestModel> DataViewWithAugments()
    {
        return new DataViewBuilder<TestModel>() {
            { m => m.BoolProperty, p => p.WithWidth(50) },
            { m => m.CurrencyProperty, p => p.WithDisabled(true) },
            { m => m.DateProperty, p => p.WithMaxValue(new DateOnly(2025, 01, 01)) },

            { m => m.DecimalProperty, p => p.WithPrecision(4) },
            { m => m.IntProperty, p => p.WithMinValue(m => m.MinValueProperty) },
            { m => m.StringListProperty, p => p.WithHidden(true) },
            { m => m.StringProperty, p => p.WithLabel("Test Label")},
            { m => m.TimeProperty, p => p.WithPropsToUpdate(x => x.BoolProperty) }
        };
    }

    private static SubPropertyGridViewBuilder<TestModel, TestModel.TestModelChild> SubPropertyGridView()
    {
        return new SubPropertyGridViewBuilder<TestModel, TestModel.TestModelChild>(m => m.EnumerableProperty)
        {
            m => m.Property1,
            m => m.Property2
        };
    }
}

public class TestModel
{
    public string StringProperty { get; set; } = "";
    public required IList<string> StringListProperty { get; set; }
    public required Currency CurrencyProperty { get; set; }
    public decimal DecimalProperty { get; set; }
    public int IntProperty { get; set; }
    public bool BoolProperty { get; set; }
    public DateOnly DateProperty { get; set; }
    public TimeOnly TimeProperty { get; set; }
    public int DisabledProperty { get; }
    internal int InternalProperty { get; set; }
    private int _noGetterField;
    public int NoGetterProperty { set => _noGetterField = value; }
    public int IntField;
    public int MinValueProperty { get; }
    public void ButtonAction() => IntField = 0;
    public IEnumerable<TestModelChild> EnumerableProperty { get; set; } = [];
    public class TestModelChild
    {
        public int Property1 { get; set; }
        public int Property2 { get; set; }
    }
}
